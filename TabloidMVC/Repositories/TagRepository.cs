using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;



namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository 
    {
        public TagRepository(IConfiguration configuration) : base(configuration) { }

        public List<Tag> GetAllTags() 
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"
                    SELECT t.Id, t.Name
                    FROM Tag t
                    ORDER BY t.Name
                    ";

                    var reader = cmd.ExecuteReader();
                    var tags = new List<Tag>();
                    while (reader.Read()) 
                    {
                        tags.Add(new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                    reader.Close();
                    return tags;
                }
            }
        }
        public Tag GetTagById(int id)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select [Name]
                        From Tag
                        Where Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    return new Tag { Id = id, Name = (string)cmd.ExecuteScalar() };
                }
            }
        }
    }   

}
