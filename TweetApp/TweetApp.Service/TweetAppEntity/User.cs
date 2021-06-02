using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TweetApp.Service.TweetAppEntity
{
    public class User
    {
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(20, ErrorMessage = "Firstname cannot be longer than 20 characters.")]
        [JsonProperty(PropertyName = "firstname")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage="Gender is required")]
        [StringLength(8, ErrorMessage = "Gender cannot be longer than 8 characters.")]
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        [JsonProperty(PropertyName = "dob")]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [JsonProperty(PropertyName = "emailId")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "Password cannot be longer than 10 characters.")]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [JsonProperty(PropertyName = "imgname")]
        public string Imgname { get; set; }
    }
}
