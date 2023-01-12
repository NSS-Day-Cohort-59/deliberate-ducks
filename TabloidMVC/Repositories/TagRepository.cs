﻿using System;
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
        public void AddTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Tag (name)
                        OUTPUT Inserted.Id
                        VALUES (@name)";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateTag(Tag tag) 
        {
            using (SqlCommand cmd = Connection.CreateCommand()) 
            {
                Connection.Open();

                using (SqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Tag
                        SET [Name] = @name
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}


