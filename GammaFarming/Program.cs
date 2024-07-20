using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Newtonsoft.Json;
using GammaFarming;
using OpenQA.Selenium.Edge;
internal class Program
{
    private static void Main(string[] args)
    {

        if (args.Length != 2)
        {
            Console.WriteLine("""
                Gamma Referral Bog Farmer
                USAGE - gamabot [referral link (string)] [number of accounts (200credits per each) (number)

                Just put your referral link as the only argument of this proyect and
                be aware to click on captcha on sign up page. Thats the only thing
                you need to do.

            """);

            Console.WriteLine(
                "Note: The whole process should be less than 50 seconds for every" +
                " account created, otherwise, this can break... (try again with better internet)");

            Console.WriteLine(
                "Note2: CAPTCHA can't be bypass by this bot, its the only thing you should" +
                " click by yourself...");

            Console.WriteLine("Finishing program");
            return;
        }

        if (!int.TryParse(args[1], out int times))
        {
            Console.WriteLine("Second argument is not a number!!!");
            Console.WriteLine("Finishing program");
        }


        //string referrallink = @"https://gamma.app/signup?r=zw517ayh03nhnzb";
        var bot = new Bot();
        bot.FarmCredits(link: args[0], times: byte.Parse(args[1]));
    }
}

