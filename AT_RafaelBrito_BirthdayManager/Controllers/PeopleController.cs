using System;
using System.Linq;
using System.Collections.Generic;
using AT_RafaelBrito_BirthdayManager.Models;
using Microsoft.AspNetCore.Mvc;
using AT_RafaelBrito_BirthdayManager.Repositories;

namespace AT_RafaelBrito_BirthdayManager.Controllers
{
    public class PeopleController : Controller
    {
        private PeopleRepository PeopleRepository { get; set; }

        public PeopleController(PeopleRepository peopleRepository)
        {
            PeopleRepository = peopleRepository;
        }

        public IActionResult Index()
        {
            List<Person> people = PeopleRepository.getAll().FindAll(person => !(person.Birthday.Day == DateTime.Now.Day && person.Birthday.Month == DateTime.Now.Month));
            List<Person> birthdayPeople = PeopleRepository.getAll().FindAll(person => person.Birthday.Day == DateTime.Now.Day && person.Birthday.Month == DateTime.Now.Month);

            people = people.OrderBy(person => person.DaysForBirthday()).ToList();

            return View(new List<List<Person>> { birthdayPeople, people });
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Person person)
        {
            Guid id = Guid.NewGuid();
            person.Id = id;
            PeopleRepository.Add(person);
            return RedirectToAction("Index", "People");
        }

        public IActionResult Search(string name)
        {
            var model = PeopleRepository.Search(name);
            return View("Dashboard", model);
        }
        

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Person person = PeopleRepository.getById(id);
            return View(person);
        }


        public IActionResult Update(Guid id)
        {
            Person person = PeopleRepository.getById(id);
            return View(person);
        }

        [HttpPost]
        public IActionResult Update(Guid id, Person person)
        {
            person.Id = id;
            PeopleRepository.Update(person);
            return RedirectToAction("Index", "People");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();
            PeopleRepository.Delete(id);
            return RedirectToAction("Index", "People");
        }
    }
}
