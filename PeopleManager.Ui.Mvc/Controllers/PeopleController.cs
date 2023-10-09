using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Requests;
using PeopleManager.Ui.Mvc.ApiServices;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PersonApiService _personApiService;

        public PeopleController(PersonApiService personApiService)
        {
            _personApiService = personApiService;
        }


        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var people = await _personApiService.Find();
            return View(people);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PersonRequest person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            var serviceResult = await _personApiService.Create(person);

            if (!serviceResult.IsSuccess)
            {
                foreach (var message in serviceResult.Messages)
                {
                    ModelState.AddModelError("", message.Title);
                }

                return View(person);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var person = await _personApiService.Get(id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            var request = new PersonRequest
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Description = person.Description
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute]int id, [FromForm]PersonRequest person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            await _personApiService.Update(id, person);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await _personApiService.Get(id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost("People/Delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _personApiService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
