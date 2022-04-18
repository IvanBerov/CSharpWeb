namespace CarShop.Models.Cars
{
    public class CarListViewModel
    {
        public string Id { get; init; }

        public string Image { get; init; }

        public string Model { get; init; }

        public int Year { get; init; }

        public string PlateNumber { get; init; }

        public int RemainingIssues { get; init; }

        public int FixedIssues { get; init; }
    }
}
