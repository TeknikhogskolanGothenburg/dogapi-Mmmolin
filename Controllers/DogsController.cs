﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dogapi_Mmmolin.Controllers
{
    [Route("[controller]")]
    public class DogsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");
            List<Models.Dog> dogs = new List<Models.Dog>();
            foreach (var file in files)
            {
                dogs.Add(JsonConvert.DeserializeObject<Models.Dog>(System.IO.File.ReadAllText(file)));
            }
            return dogs.Select(d => d.BreedName).ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");
            List<Models.Dog> dogs = new List<Models.Dog>();
            foreach (var file in files)
            {
                dogs.Add(JsonConvert.DeserializeObject<Models.Dog>(System.IO.File.ReadAllText(file)));
            }
            var dog = dogs.Where(d => d.BreedName == id).FirstOrDefault();
            if (dog == null)
            {
                return NotFound();
            }
            return new ObjectResult(dog);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Models.Dog dog)
        {
            var output = JsonConvert.SerializeObject(dog);
            if (dog == null)
            {
                Response.StatusCode = 400;
            }
            else
            {
                System.IO.File.WriteAllText(@".\DogFiles\" + dog.BreedName + ".json", output);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string breedName, [FromBody]Models.Dog dog) // take string and dog Object
        {
            if (dog == null || breedName != dog.BreedName) // If dog == null or breedName don't correspond to the object
            {
                Response.StatusCode = 400; // return bad request
            }
            var dogFile = (JsonConvert.DeserializeObject<Models.Dog>(System.IO.File.ReadAllText("/DogFiles/" + breedName + ".json")));
            dogFile.BreedName = dog.BreedName;
            dogFile.WikipediaUrl = dog.WikipediaUrl;
            dogFile.Description = dog.Description;
            var output = JsonConvert.SerializeObject(dogFile);
            System.IO.File.WriteAllText(@".\DogFiles\" + breedName + ".json", output);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}