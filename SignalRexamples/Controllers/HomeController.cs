using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRexamples.Data;
using SignalRexamples.Hubs;
using SignalRexamples.Models;
using System.Diagnostics;

namespace SignalRexamples.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<VotingHub> _votingHub;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<OrderHub> _orderHub;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IHubContext<OrderHub> orderHub,
            IHubContext<VotingHub> votingHub)
        {
            _logger = logger;
            _votingHub = votingHub;
            _context = context;
            _orderHub = orderHub;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DeathlyHallows(string? type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (Voting.DealthyHallowRace.ContainsKey(type))
                {
                    Voting.DealthyHallowRace[type]++;
                }
            }
            await _votingHub.Clients.All.SendAsync("updateDealthyHallowCount",
                Voting.DealthyHallowRace[Voting.Wand],
                Voting.DealthyHallowRace[Voting.Stone],
                Voting.DealthyHallowRace[Voting.Cloak]
                );
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Subscription()
        {
            return View();
        }
        public async Task<IActionResult> Notification()
        {
            return View();
        }

        public async Task<IActionResult> BasicChat()
        {
            return View();
        }

        [ActionName("Order")]
        public async Task<IActionResult> Order()
        {
            string[] name = { "Bhrugen", "Ben", "Jess", "Laura", "Ron" };
            string[] itemName = { "Food1", "Food2", "Food3", "Food4", "Food5" };

            Random rand = new Random();
            // Generate a random index less than the size of the array.  
            int index = rand.Next(name.Length);

            Order order = new Order()
            {
                Name = name[index],
                ItemName = itemName[index],
                Count = index
            };

            return View(order);
        }

        [ActionName("Order")]
        [HttpPost]
        public async Task<IActionResult> OrderPost(Order order)
        {

            _context.Orders.Add(order);
            _context.SaveChanges();
            await _orderHub.Clients.All.SendAsync("OrderPlaced");
            return RedirectToAction(nameof(Order));
        }
        [ActionName("OrderList")]
        public async Task<IActionResult> OrderList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAllOrder()
        {
            var productList = _context.Orders.ToList();
            return Json(new { data = productList });
        }


    }
}
