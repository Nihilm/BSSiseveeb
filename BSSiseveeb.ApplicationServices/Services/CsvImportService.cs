using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Core.Domain;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSSiseveeb.ApplicationServices.Services
{
    public class CsvImportService : ICsvImportService
    {
        public List<Employee> parseEmployees(Stream stream)
        {
            var parsedData = new List<string[]>();
            var parsedEmployees = new List<Employee>();
            string[] fields;
            TextFieldParser parser = new TextFieldParser(stream, Encoding.Default);
            parser.TextFieldType = FieldType.Delimited;
            
            parser.SetDelimiters(";");

            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();
                parsedData.Add(fields);
            }

            parser.Close();

            foreach (var user in parsedData)
            {
                try
                {
                    parsedEmployees.Add(new Employee
                    {
                        Name = user[1],
                        Email = user[2],
                        PhoneNumber = user[3],
                        Skype = user[4],
                        Birthdate = new DateTime(int.Parse(user[7]), int.Parse(user[6]), int.Parse(user[5])),
                        SocialSecurityID = user[8],
                        ContractStart = Convert.ToDateTime(user[10])
                    });
                }
                catch (Exception)
                {
                    continue;
                }

            }

            return parsedEmployees;
        }
    }
}
