using System;
using System.Collections.Generic;
using System.Data;
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

            connection.Close();
        }

        public void Update(Person pessoa)
        {
            using var connection = new SqlConnection(ConnectionString);
            string sql = "UPDATE PERSON SET FIRSTNAME = @P2, SURNAME = @P3, BIRTHDAY = @P4 WHERE Id = @P1";

            connection.Execute(sql, new { P1 = pessoa.Id, P2 = pessoa.Firstname, P3 = pessoa.Surname, P4 = pessoa.Birthday });

            connection.Close();
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(ConnectionString);
            var sql = @" DELETE FROM PERSON
                             WHERE Id = @P1 ";

            connection.Execute(sql, new { P1 = id });

            connection.Close();
        }

        public List<Person> GetBirthdayPeople()
        {
            List<Person> result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM PERSON WHERE DAY(BIRTHDAY) = DAY(@P1) AND MONTH(BIRTHDAY) = MONTH(@P1)";

                result = connection.Query<Person>(sql, new { P1 = DateTime.Now }).ToList();

                connection.Close();
            }

            return result;
        }

        public List<Person> getAllOrdered()
        {
            List<Person> result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM PERSON";

                result = connection.Query<Person>(sql).ToList();

                connection.Close();
            }
            return result;
        }

        public List<Person> GetPeopleByName(string query)
        {
            using var connection = new SqlConnection(ConnectionString);
            List<Person> people = new List<Person>();

            connection.Open();
            try
            {
                SqlCommand sqlCommand = connection.CreateCommand();

                sqlCommand.CommandText = "SELECT * FROM PERSON WHERE FIRSTNAME LIKE @p1 OR SURNAME LIKE @p1";
                sqlCommand.Parameters.AddWithValue("p1", "%" + query + "%");

                // ExecuteReader() returns data from DB but it's an Iterable.
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    people.Add(new Person(
                        Guid.Parse(sqlDataReader["ID"].ToString()),
                        sqlDataReader["FIRSTNAME"].ToString(),
                        sqlDataReader["SURNAME"].ToString(),
                        DateTime.Parse(sqlDataReader["BIRTHDAY"].ToString())
                    ));
                }

                return people;
            }
            catch
            {
                return new List<Person>();
            }
            finally
            {
                connection.Close();
            }
        }

        public Person getById(Guid id)
        {
            Person person = new Person();

            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM PERSON WHERE Id=@P1";
                person = connection.QueryFirstOrDefault<Person>(sql, new { P1 = id });

                connection.Close();
            }
            return person;
        }
    }
}
