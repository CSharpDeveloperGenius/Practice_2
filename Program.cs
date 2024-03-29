using Microsoft.EntityFrameworkCore;

namespace Practice_EF
{
    public class Program
    {
        private const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=practice;Username=postgres;Password=Gleb4ik17072005;";

        static void Main(string[] args)
        {
            using var dbContext = new ApplicationDbContext(CONNECTION_STRING);

            Task2_6(dbContext);
        }

        static void Task2_1(ApplicationDbContext dbContext)
        {
            var minPrice = 50000m;
            var maxPrice = 100000m;
            var districtTitle = "Преображенский";

            var estateObjectsQuery = dbContext.EstateObjects
                .Where(e => e.District!.Title == districtTitle
                    && e.Price >= minPrice
                    && e.Price <= maxPrice)
                .OrderByDescending(e => e.Price)
                .Select(e => new
                {
                    e.Address,
                    e.Square,
                    e.Floor
                });

            foreach (var estateObject in estateObjectsQuery)
                Console.WriteLine($"Адрес: {estateObject.Address}, Площадь: {estateObject.Square}, Этаж: {estateObject.Floor}");
        }

        static void Task2_2(ApplicationDbContext dbContext)
        {
            var roomCount = 2;

            var realtorsQuery = dbContext.Sales
                    .Where(s => s.EstateObject!.RoomCount == roomCount)
                    .Select(s => new
                    {
                        s.Agent!.LastName,
                        s.Agent.FirstName,
                        s.Agent.MiddleName
                    })
                    .Distinct();

            foreach (var realtor in realtorsQuery)
                Console.WriteLine($"Фамилия: {realtor.LastName}, Имя: {realtor.FirstName}, Отчество: {realtor.MiddleName}");
        }

        static void Task2_3(ApplicationDbContext dbContext)
        {
            var roomCount = 2;
            var districtTitle = "Преображенский";

            var totalCostQuery = dbContext.EstateObjects
                .Where(e => e.RoomCount == roomCount
                    && e.District!.Title == districtTitle)
                .Sum(e => e.Price);

            Console.WriteLine($"Общая стоимость двухкомнатных объектов недвижимости в районе '{districtTitle}': {totalCostQuery}");
        }

        static void Task2_4(ApplicationDbContext dbContext)
        {
            string lastName = "Иванов";
            string firstName = "Иван";
            string middleName = "Иванович";

            var pricesQuery = dbContext.Sales
                .Where(s => s.Agent!.LastName == lastName
                    && s.Agent.FirstName == firstName
                    && s.Agent.MiddleName == middleName)
                .Select(s => s.EstateObject!.Price);

            var maxPrice = pricesQuery.Max();
            var minPrice = pricesQuery.Min();

            Console.WriteLine($"Максимальная стоимость: {maxPrice}");
            Console.WriteLine($"Минимальная стоимость: {minPrice}");
        }

        static void Task2_5(ApplicationDbContext dbContext)
        {
            var objectType = "Апартаменты";
            var criteria = "Безопасность";
            var lastName = "Иванов";
            var firstName = "Иван";
            var middleName = "Иванович";

            var averageAssessmentQuery = dbContext.Sales
                    .Where(s => s.EstateObject!.ObjectType!.Title == objectType
                        && s.EstateObject.Assessments.Any(a => a.Criteria!.Title == criteria)
                        && s.Agent!.LastName == lastName
                        && s.Agent.FirstName == firstName
                        && s.Agent.MiddleName == middleName)
                    .SelectMany(s => s.EstateObject!.Assessments.Where(a => a.Criteria!.Title == criteria))
                    .Average(a => a.AssessmentValue);

            Console.WriteLine($"Средняя оценка апартаментов по критерию '{criteria}': {averageAssessmentQuery}");
        }

        static void Task2_6(ApplicationDbContext dbContext)
        {
            var estateObjectsCountByDistrictAndFloorQuery = dbContext.EstateObjects
                .GroupBy(e => new { e.DistrictId, e.District!.Title, e.Floor })
                .Select(g => new
                {
                    g.Key.DistrictId,
                    DistrictTitle = g.Key.Title,
                    g.Key.Floor,
                    EstateObjectsCount = g.Count()
                });

            var allDistricts = dbContext.Districts.Select(d => new { DistrictId = d.Id, DistrictTitle = d.Title }).ToList();

            var result = allDistricts
                .GroupJoin(estateObjectsCountByDistrictAndFloorQuery,
                    district => district.DistrictId,
                    query => query.DistrictId,
                    (district, query) => new
                    {
                        district.DistrictId,
                        district.DistrictTitle,
                        Floor2Count = query.Where(q => q.Floor == 2).Select(q => q.EstateObjectsCount).FirstOrDefault()
                    })
                .SelectMany(x => x.Floor2Count == 0 ? [new { x.DistrictId, x.DistrictTitle, Floor2Count = 0 }] : new[] { x });

            foreach (var item in result)
                Console.WriteLine($"Район: {item.DistrictTitle}, Количество объектов на 2 этаже: {item.Floor2Count}");
        }

        static void Task2_7(ApplicationDbContext dbContext)
        {
            var salesCountByAgentQuery = dbContext.Sales
                .GroupBy(s => new { s.Agent!.LastName, s.Agent.FirstName, s.Agent.MiddleName })
                .Select(g => new
                {
                    AgentLastName = g.Key.LastName,
                    AgentFirstName = g.Key.FirstName,
                    AgentMiddleName = g.Key.MiddleName,
                    ApartmentsSoldCount = g.Count()
                });

            foreach (var item in salesCountByAgentQuery)
                Console.WriteLine($"Риэлтор: {item.AgentLastName} {item.AgentFirstName} {item.AgentMiddleName}, Количество проданных квартир: {item.ApartmentsSoldCount}");
        }

        static void Task2_8(ApplicationDbContext dbContext)
        {
            var expensiveEstatesByDistrictQuery = dbContext.EstateObjects
                .Include(e => e.District)
                .OrderByDescending(e => e.Price)
                .ThenBy(e => e.Floor)
                .GroupBy(e => e.District!.Title)
                .ToList()
                .SelectMany(group => group
                    .Select((estate, index) => new { Estate = estate, Position = index + 1 }))
                .Where(x => x.Position <= 3)
                .OrderBy(x => x.Estate.District!.Title)
                .ThenBy(x => x.Position)
                .Select(x => new
                {
                    District = x.Estate.District!.Title,
                    Address = x.Estate.Address,
                    Price = x.Estate.Price,
                    Floor = x.Estate.Floor
                });

            foreach (var estate in expensiveEstatesByDistrictQuery)
                Console.WriteLine($"Район: {estate.District}, Адрес: {estate.Address}, Стоимость: {estate.Price}, Этаж: {estate.Floor}");
        }

        static void Task2_9(ApplicationDbContext dbContext)
        {
            var lastName = "Иванов";
            var firstName = "Иван";
            var middleName = "Иванович";

            var yearsWithMoreThanTwoSalesQuery = dbContext.Sales
                .Where(s => s.Agent!.LastName == lastName && s.Agent.FirstName == firstName && s.Agent.MiddleName == middleName)
                .GroupBy(s => s.Date.Year)
                .Where(group => group.Count() > 2)
                .Select(group => group.Key);

            Console.WriteLine($"Года, в которых риэлтор {lastName} {firstName} {middleName} продал больше 2 объектов недвижимости:");
            foreach (var year in yearsWithMoreThanTwoSalesQuery)
                Console.WriteLine(year);
        }

        static void Task2_10(ApplicationDbContext dbContext)
        {
            var yearsWithTwoToThreeEstatesQuery = dbContext.EstateObjects
                    .GroupBy(e => e.Date.Year)
                    .Where(group => group.Count() >= 2 && group.Count() <= 3)
                    .Select(group => group.Key);

            Console.WriteLine("Года, в которых было размещено от 2 до 3 объектов недвижимости:");
            foreach (var year in yearsWithTwoToThreeEstatesQuery)
                Console.WriteLine(year);
        }

        static void Task2_11(ApplicationDbContext dbContext)
        {
            var estateObjectsWithin20PercentDifferenceQuery = dbContext.EstateObjects
                .Include(e => e.District)
                .Where(e => Math.Abs(e.Price - e.Sales.OrderByDescending(s => s.Date).FirstOrDefault()!.Price) / e.Price <= 0.2m)
                .ToList();

            foreach (var estate in estateObjectsWithin20PercentDifferenceQuery)
                Console.WriteLine($"Адрес: {estate.Address}, район: {estate.District!.Title}");
        }

        static void Task2_12(ApplicationDbContext dbContext)
        {
            var apartmentsBelowAveragePriceQuery = dbContext.EstateObjects
                    .GroupBy(e => e.DistrictId)
                    .Select(g => new
                    {
                        DistrictId = g.Key,
                        AveragePricePerSquareMeter = g.Average(e => e.Price / (decimal)e.Square)
                    })
                    .Join(dbContext.EstateObjects,
                          district => district.DistrictId,
                          estate => estate.DistrictId,
                          (district, estate) => new
                          {
                              Estate = estate,
                              AveragePricePerSquareMeter = district.AveragePricePerSquareMeter
                          })
                    .Where(x => x.Estate.Price / (decimal)x.Estate.Square < x.AveragePricePerSquareMeter)
                    .Select(x => new
                    {
                        Address = x.Estate.Address,
                        DistrictId = x.Estate.DistrictId,
                        PricePerSquareMeter = x.Estate.Price / (decimal)x.Estate.Square,
                        AveragePricePerSquareMeter = x.AveragePricePerSquareMeter
                    });

            foreach (var apartment in apartmentsBelowAveragePriceQuery)
                Console.WriteLine($"Адрес: {apartment.Address}, Средняя цена за 1м2: {apartment.AveragePricePerSquareMeter}, Цена за 1м2 квартиры: {apartment.PricePerSquareMeter}");
        }

        static void Task2_13(ApplicationDbContext dbContext)
        {
            var currentYear = DateTime.Now.Year;

            var realtorsWithNoSalesThisYearQuery = dbContext.Agents
                .GroupJoin(dbContext.Sales.Where(s => s.Date.Year == currentYear),
                    agent => agent.Id,
                    sale => sale.AgentId,
                    (agent, sales) => new
                    {
                        Agent = agent,
                        SalesCount = sales.Count()
                    })
                .Where(x => x.SalesCount == 0)
                .Select(x => new
                {
                    FullName = $"{x.Agent.LastName} {x.Agent.FirstName} {x.Agent.MiddleName}"
                });

            Console.WriteLine("Риэлторы, которые ничего не продали в текущем году:");
            foreach (var realtor in realtorsWithNoSalesThisYearQuery)
                Console.WriteLine(realtor.FullName);
        }

        static void Task2_14(ApplicationDbContext dbContext)
        {
            var currentYear = DateTime.Now.Year;
            var previousYear = currentYear - 1;

            var salesByDistrictQuery = dbContext.Sales
                .Where(s => s.Date.Year == currentYear || s.Date.Year == previousYear)
                .GroupBy(s => new { DistrictTitle = s.EstateObject!.District!.Title, Year = s.Date.Year })
                .Select(group => new
                {
                    District = group.Key.DistrictTitle,
                    Year = group.Key.Year,
                    SalesCount = group.Count()
                });

            var salesData = salesByDistrictQuery
                .GroupBy(x => x.District)
                .Select(group => new
                {
                    District = group.Key,
                    CurrentYearSales = group.FirstOrDefault(x => x.Year == currentYear) != null ? group.FirstOrDefault(x => x.Year == currentYear)!.SalesCount : 0,
                    PreviousYearSales = group.FirstOrDefault(x => x.Year == previousYear) != null ? group.FirstOrDefault(x => x.Year == previousYear)!.SalesCount : 0
                });

            foreach (var salesInfo in salesData)
            {
                var percentChange = 100d;
                if (salesInfo.PreviousYearSales != 0)
                    percentChange = ((double)salesInfo.CurrentYearSales - salesInfo.PreviousYearSales) / salesInfo.PreviousYearSales * 100;

                Console.WriteLine($"Район: {salesInfo.District}, Продажи в {previousYear}: {salesInfo.PreviousYearSales}, Продажи в {currentYear}: {salesInfo.CurrentYearSales}, Изменение: {percentChange:F2}%");
            }
        }

        static void Task2_15(ApplicationDbContext dbContext)
        {
            var estateObjectId = 1;

            var averageScoresByCriteria = dbContext.Assessments
                .Where(a => a.EstateObjectId == estateObjectId)
                .GroupBy(a => a.Criteria!.Title)
                .Select(group => new
                {
                    Criteria = group.Key,
                    AverageScore = group.Average(a => a.AssessmentValue) * 20
                });

            Console.WriteLine("Средняя оценка по критериям:");
            foreach (var score in averageScoresByCriteria)
            {
                string equivalentText = GetEquivalentText(score.AverageScore);
                Console.WriteLine($"{score.Criteria}: {score.AverageScore}% - {equivalentText}");
            }
        }

        static string GetEquivalentText(double averageScore)
        {
            if (averageScore >= 90)
                return "превосходно";
            else if (averageScore >= 80)
                return "очень хорошо";
            else if (averageScore >= 70)
                return "хорошо";
            else if (averageScore >= 60)
                return "удовлетворительно";
            else
                return "неудовлетворительно";
        }

    }
}
