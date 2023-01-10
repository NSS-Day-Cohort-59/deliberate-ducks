using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        //get all users
        public List<UserProfile> GetAllUsers ()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, DisplayName, Email, CreateDateTime, UserTypeId 
                                        FROM UserProfile;";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            UserProfile user = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                Email  = reader.GetString(reader.GetOrdinal("Email")),
                                UserTypeId= reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                               CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))

                            };
                            users.Add(user);
                        }
                        return users;
                    }
                }
            }
        }
        //create new user profile
        public void AddUser(UserProfile user)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (DisplayName, FirstName, LastName, 
                                        Email, CreateDateTime, ImageLocation, UserTypeId)
                                       OUTPUT INSERTED.ID
                                        VALUES (@displayName, @firstName, @lastName, @email, 
                                            @createDateTime, @imageLocation, @userTypeId);";
                   user.CreateDateTime = DateTime.Now;
                   user.UserTypeId = 2;

                    cmd.Parameters.AddWithValue("@displayName", user.DisplayName);
                    cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@createDateTime",user.CreateDateTime);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@imageLocation", DbUtils.ValueOrDBNull( user.ImageLocation));
                    cmd.Parameters.AddWithValue("@userTypeId", user.UserTypeId);
                    
                    
                 
                    user.Id   = (int)cmd.ExecuteScalar();
                

                }
            }
        }
    }
}
