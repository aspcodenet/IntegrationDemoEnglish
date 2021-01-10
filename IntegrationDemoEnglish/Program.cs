using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using CsvHelper;
using Newtonsoft.Json;

namespace IntegrationDemoEnglish
{
    //INTEGRATIONS - Fetching data from other systems. Could also be sending of course...

    // Say we need to generate some dummy person data in a List, say 100 of them
    //Cause were gonna start coding our interface RIGHT NOW and the database implementation if far from decided

    // Create X number "fake" persons through a REST-API ... 
    // Save them into a file (append) CSV    - CSVHelper
    // Transform these file rows to  sourcecode like 
    //
    //list.Add(new Person
    //{
    //City = "Vellinge",
    //Email = "mari.lundström@leyesmessenger.shop",
    //GatuAdress = "Barbroliden 854",
    //Gender = Gender.Female,
    //Namn = "Mari Lundström",
    //PersonNummer = "19821201-3488",
    //Phone = "0857209476",
    //PostNummer = 83678
    //});

    ////
    //
    // eller (later) for example INSERT-statements to database
    //
    //
    // BUZZWORDS: INTEGRATIONS, C#, Fetch from API,  Json,. CSVHelper and NIMBLETEXT


    public enum Gender
    {
        Unknown,
        Male,
        Female
    }

    class Person
    {
        public string PersonalNumber { get; set; } // 20080528-1123 - swedish...last two are calulated from gender and checksum
                                                    //details are out of scope for this video
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }


    class NameFakePerson
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string birth_data { get; set; }
        public string phone_h { get; set; }
        public string email_d { get; set; }
        public string url { get; set; }

    }


    class Program
    {
        static void Test()
        {
            var lista = new List<Person>();
            //
            lista.Add(new Person
            {
                City = "Karlshamn",
                Email = "valfrid.arvidsson@qzick.com",
                StreetAddress = "Persson Gata 893",
                Gender = Gender.Male,
                Name = "Valfrid Arvidsson",
                PersonalNumber = "19691002-1290",
                Phone = "+46(0)409290400",
                PostalCode = 31298
            });
            lista.Add(new Person
            {
                City = "Ockelbo",
                Email = "charlotte.pettersson@detroitquote.com",
                StreetAddress = "Arvidssonbacken 97Z",
                Gender = Gender.Female,
                Name = "Charlotte Pettersson",
                PersonalNumber = "19691203-5306",
                Phone = "+46(0)607063066",
                PostalCode = 45314
            });
            lista.Add(new Person
            {
                City = "Klagstorp",
                Email = "albertina.wallin@gmailup.com",
                StreetAddress = "Hamngränd 99E",
                Gender = Gender.Female,
                Name = "Albertina Wallin",
                PersonalNumber = "19850308-6409",
                Phone = "+46 (0)111 064 040",
                PostalCode = 12353
            });
            lista.Add(new Person
            {
                City = "Årsunda",
                Email = "anny.sandström@amazonshopsource.com",
                StreetAddress = "Nyberg Allé 89",
                Gender = Gender.Female,
                Name = "Anny Sandström",
                PersonalNumber = "20020914-8622",
                Phone = "+46 (0)669 886 20",
                PostalCode = 25163
            });
            lista.Add(new Person
            {
                City = "Ytternäs och Vreta",
                Email = "teodor.norberg@allmyemployees.net",
                StreetAddress = "Annebacken 5839",
                Gender = Gender.Male,
                Name = "Teodor Norberg",
                PersonalNumber = "19870322-8058",
                Phone = "024775547",
                PostalCode = 16337
            });
            lista.Add(new Person
            {
                City = "Hjärnarp",
                Email = "billy.åkesson@amazonshopsite.com",
                StreetAddress = "Vasaliden 80",
                Gender = Gender.Male,
                Name = "Billy Åkesson",
                PersonalNumber = "19890304-4230",
                Phone = "+46(0)4512230",
                PostalCode = 61167
            });
            lista.Add(new Person
            {
                City = "Gåvsta",
                Email = "sara.lundberg@memoryclub.hk",
                StreetAddress = "Fredsliden 5842",
                Gender = Gender.Female,
                Name = "Sara Lundberg",
                PersonalNumber = "19820831-6309",
                Phone = "+46298274375",
                PostalCode = 61053
            });
            lista.Add(new Person
            {
                City = "Vilhelmina",
                Email = "emelia.andersson@dealyoyo.com",
                StreetAddress = "Gustafssonbacken 859",
                Gender = Gender.Female,
                Name = "Emelia Andersson",
                PersonalNumber = "19930813-1887",
                Phone = "08-404 848 88",
                PostalCode = 24514
            });
            lista.Add(new Person
            {
                City = "Tjuvkil",
                Email = "david.magnusson@dfnqxymfm.cf",
                StreetAddress = "Ringbacken 10J",
                Gender = Gender.Male,
                Name = "David Magnusson",
                PersonalNumber = "19900118-1213",
                Phone = "+46 (0)6457273",
                PostalCode = 11752
            });
            lista.Add(new Person
            {
                City = "Hovmantorp",
                Email = "joakim.axelsson@gooses.design",
                StreetAddress = "Runegatan 3",
                Gender = Gender.Male,
                Name = "Joakim Axelsson",
                PersonalNumber = "20020616-2253",
                Phone = "052-832 54 18",
                PostalCode = 64408
            });


            foreach (var person in lista)
            {
                Console.WriteLine($"{person.Name} {person.PersonalNumber} ");
            }

        }

        static void Main(string[] args)
        {
            Test();

            using (var wc = new WebClient())
            {
                for (int i = 0; i < 10; i++)
                {
                    var data = wc.DownloadString("https://api.namefake.com/swedish-sweden/random/");

                    var nameFakePerson = JsonConvert.DeserializeObject<NameFakePerson>(data);
                    var person = GeneratePersonFrom(nameFakePerson);

                    using (var writer = new StreamWriter(".\\persons.csv", true))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecord(person);
                            writer.Flush();
                            writer.Write(Environment.NewLine);
                        }
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }


            Console.WriteLine("Hello World!");
        }

        private static Person GeneratePersonFrom(NameFakePerson f1)
        {
            var dat = DateTime.ParseExact(f1.birth_data, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            var addressParts = f1.Address.Split('\n');
            var cityParts = addressParts[1].Split(' ');

            var postal = cityParts[0];
            var city = string.Join(' ', cityParts.Skip(1));

            if (cityParts.Length == 3)
            {
                postal = cityParts[0] + cityParts[1];
                city = string.Join(' ', cityParts.Skip(2));
            }

            var gender = f1.url.Contains("female") ? Gender.Female : Gender.Male;

            var p = new Person
            {
                Name = f1.Name,
                Email = Emailify(f1.Name) + "@" + f1.email_d,
                Phone = f1.phone_h,
                City = city,
                PostalCode = Convert.ToInt32(postal),
                StreetAddress = addressParts[0],
                PersonalNumber = GeneratePersonNummer(dat, gender),
                Gender = gender
            };
            return p;
        }

        private static Random rand = new Random();
        private static string GeneratePersonNummer(DateTime dat, Gender gender)
        {
            var odd = new[] { "1", "3", "5", "7", "9" };
            var even = new[] { "2", "4", "6", "8", "0" };

            var start = dat.ToString("yyyyMMdd");

            start += rand.Next(0, 100).ToString("00");

            if (gender == Gender.Male)
                start += odd[rand.Next(5)];
            if (gender == Gender.Female)
                start += even[rand.Next(5)];

            start += Convert.ToString(Luhn(start));

            return start.Substring(0, 8) + "-" + start.Substring(8);
        }



        private static int Luhn(string value)
        {
            // Luhm algorithm doubles every other number in the value.
            // To get the correct checksum digit we aught to append a 0 on the sequence.
            // If the result becomes a two digit number, subtract 9 from the value.
            // If the total sum is not a 0, the last checksum value should be subtracted from 10.
            // The resulting value is the check value that we use as control number.

            // The value passed is a string, so we aught to get the actual integer value from each char (i.e., subtract '0' which is 48).
            int[] t = value.ToCharArray().Select(d => d - 48).ToArray();
            int sum = 0;
            int temp;
            for (int i = 0; i < t.Length; i++)
            {
                temp = t[i];
                temp *= 2 - (i % 2);
                if (temp > 9)
                {
                    temp -= 9;
                }
                sum += temp;
            }

            return ((int)Math.Ceiling(sum / 10.0)) * 10 - sum;
        }


        private static string Emailify(string f1Name)
        {
            var v = "";
            foreach (char ch in f1Name)
                if (char.IsLetter(ch))
                    v += char.ToLower(ch);
                else if (ch == ' ') v += '.';
            return v;
        }
    }
}
