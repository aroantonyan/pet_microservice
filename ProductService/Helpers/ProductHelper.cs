namespace ProductService.Helpers;

public abstract class ProductHelper
{
    public static double AdjustPriceIfDesirable(string productName, double productPrice)
    {
        var desirableProducts = new List<string>(){"Iphone", "IMac", "IPad"};
        foreach (var item in desirableProducts)
        {
            if (string.Equals(productName, item))
            {
                productPrice +=50;
            }
        }
        return productPrice;
    }
}