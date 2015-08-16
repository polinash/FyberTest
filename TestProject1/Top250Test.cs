using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Top250Test: BaseTest
    {
        private string TestedUrl = "http://www.imdb.com/chart/top";

#region elements
        //top250 movie list
        private string MovielistElement
		{
			get { return "//div[@class='lister']//tbody[@class='lister-list']/tr/td"; }
		}

        //top250 SortBy control
        private string MovielistSorter
		{
			get { return "//div[@class='lister']//select[@class='lister-sort-by']"; }
		}

        //top250 SortbyControl element
        private string MovielistSorterOption (int i)
        {
             return String.Format("//div[@class='lister']//select[@class='lister-sort-by']/option[{0}]",i); 
        }

        //top250 Asc/Desc sortr control
        private string MovielistDirSorter
		{
			get { return "//div[@class='lister']//span"; }
		}

        //top250 western genre link - can easily be changed to a method for any genre. don't need it now
        private string Western
        {
            get { return "//li[@class='subnav_item_main']/a"; }
        }//ul[@class='quicklinks']
#endregion

        //Task 1. I just check whether the first element of the movie list is present. 
        //if it is not - we know there are no elements at all
        [TestMethod]
        public void NotEmptyListTest()
        {
            Navigate(TestedUrl);
           AssertElementPresent(MovielistElement,"There are no items in the list or list not present.");
        }

        //Task2. I need to make the same test as before but choosing different 
        // sortings every time through clicking on the elements of the UI
        //I have multiple asserts there so i accumulate errors
        [TestMethod]
        public void NotEmptySortedListTest()
        {
            Navigate(TestedUrl);
            //check there are controls for sorting
            AssertElementPresent(MovielistSorter, "There is sorter.");
            AssertElementPresent(MovielistDirSorter, "There is no asc/desc sorter.");
            
            StringBuilder result =  new StringBuilder();

                for (int i = 1; i < 6; i++)
                {
                    try
                    {
                            //check diff sort by
                            ClickElement(MovielistSorter);
                            ClickElement(MovielistSorterOption(i));
                            AssertElementPresent(MovielistElement, String.Format("Sort By : {0}, {1}. There are no items in the list or list not present.", GetElementText(MovielistSorter), GetElementText(MovielistDirSorter)));
                    }
                    catch (Exception e)
                    {result.Append(e.Message);}
                    try
                    {
                        //check diff desc-asc 
                        ClickElement(MovielistDirSorter);
                        AssertElementPresent(MovielistElement, String.Format("Sort By : {0}, {1}. There are no items in the list or list not present.", GetElementText(MovielistSorter), GetElementText(MovielistDirSorter)));
                    }
                    catch (Exception e)
                    { result.Append(e.Message); }
                }

                result.ToString().ShouldBeNullOrEmpty(result.ToString());

        }

        //same task2 but using knowledge of the parameters in the URL
        //http://www.imdb.com/chart/top?sort=us,asc&mode=simple&page=1
        [TestMethod]
        public void NotEmptySortedListTest2()
        {
            string TestUrl = "http://www.imdb.com/chart/top?sort={0},{1}&mode=simple&page=1";
            string[] SortBy = {"rk", "ir","nv", "us", "ur"};
            string[] SortDir = { "asc", "desc"};

            
            StringBuilder result = new StringBuilder();
            foreach (string sby in SortBy)
            {
                foreach (string sd in SortDir)
                {
                    Navigate(String.Format(TestUrl,sby,sd));
                    try
                    {
                        AssertElementPresent(MovielistElement, String.Format("{0},{1}. There are no items in the list or list not present.", sby, sd));
                    }
                    catch (Exception e)
                    {
                        result.Append(e.Message);
                    }
                }
            }

            if (result.Length > 0)
                throw new Exception(result.ToString());

        }

        //Task 3. I need to make the same test as before but choosing Western​ genre
        [TestMethod]
        public void NotEmptyListWesternGenreTest()
        {
            Navigate(TestedUrl);

            ClickElement(Western, "Western");
            //only use it once here - western top list element
            string MovielistElement = "//table[@class='results']/tbody/tr/td";

            AssertElementPresent(MovielistElement, "There are no items in the list or list not present.");
        }

    }
}
