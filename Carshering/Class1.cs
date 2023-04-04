public class Car
{
    private string brand;
    private string model;
    private int price_per_hour;

    public Car(string brand, string model, int price_per_hour)
    {
        this.brand = brand;
        this.model = model;
        this.price_per_hour = price_per_hour;
    }

    public string Brand
    {
        get { return brand; }
        set { brand = value; }
    }

    public string Model
    {
        get { return model; }
        set { model = value; }
    }

    public int PricePerHour
    {
        get { return price_per_hour; }
        set { price_per_hour = value; }
    }
}