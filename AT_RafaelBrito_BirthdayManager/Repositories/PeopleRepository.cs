using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AT_RafaelBrito_BirthdayManager.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace AT_RafaelBrito_BirthdayManager.Repositories
{
    public class PeopleRepository
    {
        private string ConnectionString { get; set; }
        public PeopleRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DBConnection");

        }
        public void Add(Person person)
        {
            using var connection = new SqlConnection(ConnectionString);
            string sql = "INSERT INTO PERSON(ID, FIRSTNAME, SURNAME, BIRTHDAY) VALUES(@P1,@P2,@P3,@P4)";

            connection.Execute(sql, new { P1 = person.Id, P2 = person.Firstname, P3 = person.Surname, P4 = person.Birthday });
        }

        public void Update(Person pessoa)
        {
            using var connection = new SqlConnection(ConnectionString);
            string sql = "UPDATE PERSON SET FIRSTNAME = @P2, SURNAME = @P3, BIRTHDAY = @P4 WHERE Id = @P1";

            connection.Execute(sql, new { P1 = pessoa.Id, P2 = pessoa.Firstname, P3 = pessoa.Surname, P4 = pessoa.Birthday });
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(ConnectionString);
            var sql = @" DELETE FROM PERSON
                             WHERE Id = @P1 ";

            connection.Execute(sql, new { P1 = id });
        }

        public List<Person> getAll()
        {
            List<Person> result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM PERSON";

                result = connection.Query<Person>(sql).ToList();
            }
            return result;
        }

        public List<Person> Search(string query)
        {
            List<Person> result;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = @"
                        SELECT ID, FIRSTNAME, SURNAME, BIRTHDAY
                        FROM PERSON
                        WHERE (FIRSTNAME LIKE '%" + query + "%' OR SURNAME LIKE '%" + query + "%')";

                result = connection.Query<Person>(sql).ToList();
            }

            return result;
        }

        public Person getById(Guid id)
        {
            Person person = new Person();

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM PERSON WHERE Id=@P1";
                person = connection.QueryFirstOrDefault<Person>(sql, new { P1 = id });
            }
            return person;
        
        }
    }
}
