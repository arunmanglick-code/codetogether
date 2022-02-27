using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Xml.Linq;



namespace Arun.Manglick.Silverlight
{
    public partial class DiggSample : UserControl
    {
        public DiggSample()
        {
            InitializeComponent();
        }

        void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //String diggUrl = String.Format("http://services.digg.com/stories/topic/{0}?count=20&appkey=http%3A%2F%2Fscottgu.com", topic);
                String diggUrl = "http://services.digg.com/topics?appkey=http%3A%2F%2Fapidoc.digg.com";

                // Initiate Async Network call to Digg
                WebClient diggService = new WebClient();
                diggService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DiggService_DownloadStoriesCompleted);
                diggService.DownloadStringAsync(new Uri(diggUrl));
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        void SearchStories_Click(object sender, RoutedEventArgs e)
        {
            //String diggUrl = "http://localhost/Arun.Manglick.Silverlight/XML/DiggSample.xml";

            //// Initiate Async Network call to Digg
            //WebClient diggService = new WebClient();
            //diggService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DiggService_DownloadLongStoriesCompleted);
            //diggService.DownloadStringAsync(new Uri(diggUrl));
            String xmlString = @"<?xml version='1.0' encoding='utf-8' ?>
                            <stories timestamp='1196894147' min_date='1194302130' total='2636' offset='0' count='3'>
                              <story id='4368401' link='http://maxsangalli.altervista.org/?p=45' submit_date='1196891534' diggs='1' comments='0' status='upcoming' media='news' href='http://digg.com/linux_unix/Jukebox_con_Linux'>
                                <title>Jukebox con Linux</title>
                                <description>Jukebox with Linux</description>
                                <user name='ilsanga' icon='http://digg.com/img/udl.png' registered='1196891377' profileviews='0' />
                                <topic name='Linux/Unix' short_name='linux_unix' />
                                <container name='Technology' short_name='technology' />
                                <thumbnail originalwidth='390' originalheight='387' contentType='image/gif' src='http://localhost/Arun.Manglick.Silverlight/Images/MonetricsSmall.JPG' width='80' height='80' />
                              </story>
                              <story id='4367880' link='http://www.itsyourip.com/ip-tools/arping-ping-using-address-resolution-protocolarp/' submit_date='1196888736' diggs='2' comments='0' status='upcoming' media='news' href='http://digg.com/linux_unix/ARPing_Ping_using_Address_Resolution_Protocol_ARP'>
                                <title>ARPing - Ping using Address Resolution Protocol(ARP)</title>
                                <description>ARPing is an utility that can be used to ping using Address Resolution Protocol (ARP) requests instead of using Internet Control Message Protocol (ICMP) as used in standard Ping utility. ARPing can be used for Duplicate Address Detection (DAD)</description>
                                <user name='rbkumaran' icon='http://digg.com/users/rbkumaran/l.png' registered='1151050300' profileviews='42' />
                                <topic name='Linux/Unix' short_name='linux_unix' />
                                <container name='Technology' short_name='technology' />
                                <thumbnail originalwidth='390' originalheight='387' contentType='image/gif' src='http://localhost/Arun.Manglick.Silverlight/Images/MonetricsSmall.JPG' width='80' height='80' />
                              </story>
                              <story id='4367579' link='http://linuxdevices.com/news/NS9372729455.html' submit_date='1196887377' diggs='5' comments='0' status='upcoming' media='news' href='http://digg.com/linux_unix/EMF_changes_tune_hails_embedded_Linux'>
                                <title>EMF changes tune, hails embedded Linux </title>
                                <description>Embedded Market Forecasters has issued a report claiming that embedded Linux is as dependable as other real time operating systems (RTOSs). The independently funded report appears to recant EMF's controversial Microsoft-funded report in 2003 that claimed that embedded Windows OSes were far faster and cheaper than embedded Linux. ...</description>
                                <user name='gadgeek' icon='http://digg.com/img/udl.png' registered='1131035306' profileviews='1717' />
                                <topic name='Linux/Unix' short_name='linux_unix' />
                                <container name='Technology' short_name='technology' />
                                <thumbnail originalwidth='125' originalheight='76' contentType='image/gif' src='http://localhost/Arun.Manglick.Silverlight/Images/MonetricsSmall.JPG' width='80' height='80' />
                              </story>
                            </stories>
                            ";

            DisplayLongStories(xmlString);

        }

        void DiggService_DownloadStoriesCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DisplayStories(e.Result);                
            }
            else
            {
                txtResult.Text = ((System.Exception)e.Error.InnerException).Message;
            }
        }

        void DiggService_DownloadLongStoriesCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DisplayLongStories(e.Result);
            }
            else
            {
                txtResult.Text = ((System.Exception)e.Error.InnerException).Message;
            }
        }

        void DisplayStories(string xmlContent)
        {
            XDocument xmlTopics = XDocument.Parse(xmlContent);
            String topicText = txtSearchTopic.Text;

            if (topicText != String.Empty)
            {
                var topics = from topic in xmlTopics.Descendants("topic")
                             where topic.Element("container").Attribute("name").Value.StartsWith(topicText)
                             select new DiggTopic
                             {
                                 Name = (string)topic.Attribute("name"),
                                 Technology = (string)topic.Element("container").Attribute("name"),
                             };
                TopicsList.SelectedIndex = -1;
                TopicsList.ItemsSource = topics;
            }
            else
            {
                var topics = from topic in xmlTopics.Descendants("topic")                            
                             select new DiggTopic
                             {
                                 Name = (string)topic.Attribute("name"),
                                 Technology = (string)topic.Element("container").Attribute("name"),
                             };

                TopicsList.SelectedIndex = -1;
                TopicsList.ItemsSource = topics;
            }
        }

        void DisplayLongStories(string xmlContent)
        {

            XDocument xmlStories = XDocument.Parse(xmlContent);
            

            var stories = from story in xmlStories.Descendants("story")
                          where story.Element("thumbnail") != null 
                          select new DiggStory
                          {
                              Id = (int)story.Attribute("id"),
                              Title = ((string)story.Element("title")).Trim(),
                              Description = ((string)story.Element("description")).Trim(),
                              ThumbNail = (string)story.Element("thumbnail").Attribute("src").Value,
                              HrefLink = new Uri((string)story.Attribute("link")),
                              NumDiggs = (int)story.Attribute("diggs"),
                              UserName = (string)story.Element("user").Attribute("name").Value,
                          };

            StoriesList.SelectedIndex = -1;
            StoriesList.ItemsSource = stories;
        }

        void StoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DiggStory story = (DiggStory)StoriesList.SelectedItem;
            //        <Digg:StoriesDetailsUC x:Name="DetailsView" Grid.Row="4" Visibility="Collapsed"></Digg:StoriesDetailsUC>


            if (story != null)
            {
                //DetailsView.DataContext = story;
                //DetailsView.Visibility = Visibility.Visible;
            }
        }
    }
}

