using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Net;
using System.IO;
//
using System.Runtime.InteropServices; // uses the "HtmlAgilityPack" NuGet package library package.     [Visual Studio] - [top bar] - [Project] - [manage NuGet packages...] - [search]
//


namespace TravisScraper_WpfApp
{
    internal static class WebpageScraper_Class
    {
        public static string ReadUrlData(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());

            string UrlCode = sr.ReadToEnd();

            return (UrlCode);
        }










        public static string[] FindYoutubeVideoMetaDataFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            //text.text = sr.ReadToEnd();


            string originalUrlCode = sr.ReadToEnd();


            string croppedUrlCode = CropTheOriginalUrlCode(originalUrlCode);
            originalUrlCode = "-empty-"; // dont need this after this point


            //cut up the first section [title, description etc]

            string[] tempStringArray = CutUpUrlCodeBlocks(croppedUrlCode);
            string block1 = tempStringArray[0];
            string block2 = tempStringArray[1];
            string block3 = tempStringArray[2];
            tempStringArray[0] = "-empty-"; // dont need this after this point
            tempStringArray[1] = "-empty-";
            tempStringArray[2] = "-empty-";
            croppedUrlCode = "-empty-"; // dont need this after this point



            //-------published, uploaded------------------------------------------------------
            //Input:
            {
                /*
                tempSecond

                datePublished" content="2021-07-02"><meta itemprop="uploadDate" content="2021-07-02"><meta itemprop="genre" content="Music"></div></head><body dir="ltr" no-y-overflow><script nonce="dxg7P0BTZ8l0akfjZCEywg">var 
                */
            }

            int datePublishedIndex = block1.IndexOf("datePublished");
            string datePublishedString = block1.Substring(datePublishedIndex + 24, 10);

            int dateUploadedIndex = block1.IndexOf("uploadDate");
            string dateUploadedString = block1.Substring(dateUploadedIndex + 21, 10);

            //Output:
            {
                /*
                datePublished   = 2021-07-02
                uploadDate      = 2021-07-02
                */
            }
            //-------------------------------------------------------------


            //-------thumbnailUrl, channelName------------------------------------------------------
            //Input:
            {
                /*
                tempThird

                title":"𝐩𝐨𝐯: 𝐬𝐥𝐞𝐞𝐩𝐨𝐯𝐞𝐫 𝐰𝐢𝐭𝐡 𝐓𝐨𝐦𝐦𝐲 || a Tommyinnit playlist ♡","lengthSeconds":"3623","keywords":["dsmp","dreamsmp","dream smp","tommyinnit","tommyinnit playlist","playlist","dreamsmp playlist","dsmp playlist","dream smp playlist","raccooninnit","tommy innit","tommy innit playlist"],"channelId":"UC_ptvA3rNT5FmAqG_06QHlg","isOwnerViewing":false,"shortDescription":"!! ART IS BY ULTIMATEDIRK ON TUMBLR, HOWEVER THEY DO NOT WANT TO BE ASSOCIATED WITH DSMP ANY LONGER AND HAVE SINCE DELETED THE POST. HERE IS A REBLOG: https://b-e-e-e-s.tumblr.com/post/630043986183192576/first-drawing-with-my-laptop-fixed-have-a !!\na tommy playlist to hold you guys over!! my wilbur one went haywire and i felt a lil discouraged but hopefully this one will be okay :D !!","isCrawlable":true,"thumbnail":{"thumbnails":[{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEiCKgBEF5IWvKriqkDFQgBFQAAAAAYASUAAMhCPQCAokN4AQ==\u0026rs=AOn4CLBiNt2-xwC6Y1S_4v25Yq6LOMWqHw","width":168,"height":94},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEiCMQBEG5IWvKriqkDFQgBFQAAAAAYASUAAMhCPQCAokN4AQ==\u0026rs=AOn4CLBCCMZmeHuU0GTfzJp-nrFduzLDxg","width":196,"height":110},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEjCPYBEIoBSFryq4qpAxUIARUAAAAAGAElAADIQj0AgKJDeAE=\u0026rs=AOn4CLCPFIAngYzw_riwJv6eiW2mhI6RpQ","width":246,"height":138},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEjCNACELwBSFryq4qpAxUIARUAAAAAGAElAADIQj0AgKJDeAE=\u0026rs=AOn4CLD9y-2pqtnKKQbycBx_PZzRie-raQ","width":336,"height":188},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/maxresdefault.jpg","width":1920,"height":1080}]},"averageRating":5,"allowRatings":true,"viewCount":"989","author":"littlecubesandtea","isPrivate":false,"isUnpluggedCorpus":false,"isLiveContent":false},"annotations":[{"playerAnnotationsExpandedRenderer":{"featuredChannel":{"startTimeMs":"0","endTimeMs":"3602000","watermark":{"thumbnails":[{"url":"https://i.ytimg.com/an/_ptvA3rNT5FmAqG_06QHlg/featured_channel.jpg?v=60ac60d1","width":40,"height":40}]},"trackingParams":"CBIQ8zciEwjdp7ieudjxAhVr_zgGHd48Bdc=","navigationEndpoint":{"clickTrackingParams":"CBIQ8zciEwjdp7ieudjxAhVr_zgGHd48BdcyAml2","commandMetadata":{"webCommandMetadata":{"url":"/channel/UC_ptvA3rNT5FmAqG_06QHlg","webPageType":"WEB_PAGE_TYPE_CHANNEL","rootVe":3611,"apiUrl":"/youtubei/v1/browse"}},"browseEndpoint":{"browseId":"UC_ptvA3rNT5FmAqG_06QHlg"}},"channelName":"littlecubesandtea","
                */
            }

            int thumbnailUrlIndex1 = block2.IndexOf("188");
            string thumbnailUrlString = block2.Substring(thumbnailUrlIndex1 + 13, block2.Length - (thumbnailUrlIndex1 + 13));
            int thumbnailUrlIndex2 = thumbnailUrlString.IndexOf(",");
            thumbnailUrlString = thumbnailUrlString.Substring(0, thumbnailUrlIndex2 - 1);

            //int channelNameIndex1 = tempThird.IndexOf("channelName");
            //string channelNameString = tempThird.Substring(channelNameIndex1 + 14, tempThird.Length - (channelNameIndex1 + 14));
            //int channelNameIndex2 = channelNameString.IndexOf(",");
            //channelNameString = channelNameString.Substring(0, channelNameIndex2 - 1);

            //Output:
            {
                /*
                thumbnailUrl    = https://i.ytimg.com/vi/Wp6W3RBkQac/maxresdefault.jpg
                channelName     = littlecubesandtea
                */
            }
            //-------------------------------------------------------------


            //-------Title, Desc, Durat, Owner------------------------------------------------------
            //Input:
            {
                /*
                block3

                title":{"simpleText":"𝐩𝐨𝐯: 𝐬𝐥𝐞𝐞𝐩𝐨𝐯𝐞𝐫 𝐰𝐢𝐭𝐡 𝐓𝐨𝐦𝐦𝐲 || a Tommyinnit playlist ♡"},"description":{"simpleText":"!! ART IS BY ULTIMATEDIRK ON TUMBLR, HOWEVER THEY DO NOT WANT TO BE ASSOCIATED WITH DSMP ANY LONGER AND HAVE SINCE DELETED THE POST. HERE IS A REBLOG: https://b-e-e-e-s.tumblr.com/post/630043986183192576/first-drawing-with-my-laptop-fixed-have-a !!\na tommy playlist to hold you guys over!! my wilbur one went haywire and i felt a lil discouraged but hopefully this one will be okay :D !!"},"lengthSeconds":"3623","ownerProfileUrl":"http://www.youtube.com/channel/UC_ptvA3rNT5FmAqG_06QHlg","
                */
            }

            int titleIndex1 = block3.IndexOf("title");
            string titleString = block3.Substring(titleIndex1 + 22, block3.Length - (titleIndex1 + 22));
            int titleIndex2 = titleString.IndexOf("}");
            titleString = titleString.Substring(0, titleIndex2 - 1);

            int descriptionIndex1 = block3.IndexOf("description");
            string descriptionString = block3.Substring(descriptionIndex1 + 28, block3.Length - (descriptionIndex1 + 28));
            int descriptionIndex2 = descriptionString.IndexOf("}");
            descriptionString = descriptionString.Substring(0, descriptionIndex2 - 1);

            int durationIndex1 = block3.IndexOf("lengthSeconds");
            string durationString = block3.Substring(durationIndex1 + 16, block3.Length - (durationIndex1 + 16));
            int durationIndex2 = durationString.IndexOf(",");
            durationString = durationString.Substring(0, durationIndex2 - 1);

            int ownerProfileUrlIndex1 = block3.IndexOf("ownerProfileUrl");
            string ownerProfileUrlString = block3.Substring(ownerProfileUrlIndex1 + 18, block3.Length - (ownerProfileUrlIndex1 + 18));
            int ownerProfileUrlIndex2 = ownerProfileUrlString.IndexOf(",");
            ownerProfileUrlString = ownerProfileUrlString.Substring(0, ownerProfileUrlIndex2 - 1);

            //Output:
            {
                /*
                title           = 𝐩𝐨𝐯: 𝐬𝐥𝐞𝐞𝐩𝐨𝐯𝐞𝐫 𝐰𝐢𝐭𝐡 𝐓𝐨𝐦𝐦𝐲 || a Tommyinnit playlist ♡
                description     = !! ART IS BY ULTIMATEDIRK ON TUMBLR, HOWEVER THEY DO NOT WANT TO BE ASSOCIATED WITH DSMP ANY LONGER AND HAVE SINCE DELETED THE POST. HERE IS A REBLOG: https://b-e-e-e-s.tumblr.com/post/630043986183192576/first-drawing-with-my-laptop-fixed-have-a !!\na tommy playlist to hold you guys over!! my wilbur one went haywire and i felt a lil discouraged but hopefully this one will be okay :D !!
                duration        = 3623
                ownerProfileUrl = http://www.youtube.com/channel/UC_ptvA3rNT5FmAqG_06QHlg
                */
            }
            //-------------------------------------------------------------


            //-------Invent video ID------------------------------------------------------
            //Input:
            {
                /*
                thumbnailUrlString

                thumbnailUrl    = https://i.ytimg.com/vi/Wp6W3RBkQac/maxresdefault.jpg
                */
            }

            int videoIDIndex1 = thumbnailUrlString.IndexOf("vi");
            string videoIDString = thumbnailUrlString.Substring(videoIDIndex1 + 3, thumbnailUrlString.Length - (videoIDIndex1 + 3));
            int videoIDIndex2 = videoIDString.IndexOf("maxresdefault");
            videoIDString = videoIDString.Substring(0, videoIDIndex2 - 1);

            //Output:
            {
                /*
                thumbnailUrl    = https://i.ytimg.com/vi/Wp6W3RBkQac/maxresdefault.jpg
                channelName     = littlecubesandtea
                */
            }
            //-------------------------------------------------------------


            string[] outputStringArray = { videoIDString, datePublishedString, dateUploadedString, thumbnailUrlString, "channelName", titleString, descriptionString, durationString, url, ownerProfileUrlString };

            return outputStringArray;
        }


        static string CropTheOriginalUrlCode(string theOriginalUrlCode)
        {
            int startOfCropIndex = theOriginalUrlCode.IndexOf("datePublished");
            int endOfCropIndex = theOriginalUrlCode.IndexOf("externalChannelId");
            string theCroppedUrlCode = theOriginalUrlCode.Substring(startOfCropIndex, endOfCropIndex);

            return theCroppedUrlCode;
        }


        static string[] CutUpUrlCodeBlocks(string theCroppedUrlCode)
        {
            int endOfblock1Index = theCroppedUrlCode.IndexOf("ytInitialPlayerResponse");
            string block1 = theCroppedUrlCode.Substring(0, endOfblock1Index); //create block 1

            string block2 = theCroppedUrlCode.Substring(endOfblock1Index, theCroppedUrlCode.Length - endOfblock1Index); //start by removing block 1 from block 2

            int startOfBlock2Index = block2.IndexOf("title"); //then start cutting at the actual beginning of block 2
            block2 = block2.Substring(startOfBlock2Index, block2.Length - startOfBlock2Index);
            string block3 = block2;
            int endOfBlock2Index = block2.IndexOf("1920");
            block2 = block2.Substring(0, endOfBlock2Index);


            block3 = block3.Substring(endOfBlock2Index, block3.Length - endOfBlock2Index);
            int startOfBlock3Index = block3.IndexOf("title");
            block3 = block3.Substring(startOfBlock3Index, block3.Length - startOfBlock3Index);

            // Output 2:
            { // Output:
                /*
                   datePublished" content="2021-07-02"><meta itemprop="uploadDate" content="2021-07-02"><meta itemprop="genre" content="Music"></div></head><body dir="ltr" no-y-overflow><script nonce="CQvwubc5NFA0L+aQDz22GQ">var 

                   ------------SPLIT------------

                   title":"𝐩𝐨𝐯: 𝐬𝐥𝐞𝐞𝐩𝐨𝐯𝐞𝐫 𝐰𝐢𝐭𝐡 𝐓𝐨𝐦𝐦𝐲 || a Tommyinnit playlist ♡","lengthSeconds":"3623","keywords":["dsmp","dreamsmp","dream smp","tommyinnit","tommyinnit playlist","playlist","dreamsmp playlist","dsmp playlist","dream smp playlist","raccooninnit","tommy innit","tommy innit playlist"],"channelId":"UC_ptvA3rNT5FmAqG_06QHlg","isOwnerViewing":false,"shortDescription":"!! ART IS BY ULTIMATEDIRK ON TUMBLR, HOWEVER THEY DO NOT WANT TO BE ASSOCIATED WITH DSMP ANY LONGER AND HAVE SINCE DELETED THE POST. HERE IS A REBLOG: https://b-e-e-e-s.tumblr.com/post/630043986183192576/first-drawing-with-my-laptop-fixed-have-a !!\na tommy playlist to hold you guys over!! my wilbur one went haywire and i felt a lil discouraged but hopefully this one will be okay :D !!","isCrawlable":true,"thumbnail":{"thumbnails":[{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEiCKgBEF5IWvKriqkDFQgBFQAAAAAYASUAAMhCPQCAokN4AQ==\u0026rs=AOn4CLBiNt2-xwC6Y1S_4v25Yq6LOMWqHw","width":168,"height":94},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEiCMQBEG5IWvKriqkDFQgBFQAAAAAYASUAAMhCPQCAokN4AQ==\u0026rs=AOn4CLBCCMZmeHuU0GTfzJp-nrFduzLDxg","width":196,"height":110},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEjCPYBEIoBSFryq4qpAxUIARUAAAAAGAElAADIQj0AgKJDeAE=\u0026rs=AOn4CLCPFIAngYzw_riwJv6eiW2mhI6RpQ","width":246,"height":138},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/hqdefault.jpg?sqp=-oaymwEjCNACELwBSFryq4qpAxUIARUAAAAAGAElAADIQj0AgKJDeAE=\u0026rs=AOn4CLD9y-2pqtnKKQbycBx_PZzRie-raQ","width":336,"height":188},{"url":"https://i.ytimg.com/vi/Wp6W3RBkQac/maxresdefault.jpg","width":1920,"height":1080}]},"averageRating":5,"allowRatings":true,"viewCount":"743","author":"littlecubesandtea","isPrivate":false,"isUnpluggedCorpus":false,"isLiveContent":false},"annotations":[{"playerAnnotationsExpandedRenderer":{"featuredChannel":{"startTimeMs":"0","endTimeMs":"3602000","watermark":{"thumbnails":[{"url":"https://i.ytimg.com/an/_ptvA3rNT5FmAqG_06QHlg/featured_channel.jpg?v=60ac60d1","width":40,"height":40}]},"trackingParams":"CBIQ8zciEwjnva2YidbxAhWrwaACHdMEBE4=","navigationEndpoint":{"clickTrackingParams":"CBIQ8zciEwjnva2YidbxAhWrwaACHdMEBE4yAml2","commandMetadata":{"webCommandMetadata":{"url":"/channel/UC_ptvA3rNT5FmAqG_06QHlg","webPageType":"WEB_PAGE_TYPE_CHANNEL","rootVe":3611,"apiUrl":"/youtubei/v1/browse"}},"browseEndpoint":{"browseId":"UC_ptvA3rNT5FmAqG_06QHlg"}},"channelName":"littlecubesandtea","

                   ------------SPLIT------------

                   title":{"simpleText":"𝐩𝐨𝐯: 𝐬𝐥𝐞𝐞𝐩𝐨𝐯𝐞𝐫 𝐰𝐢𝐭𝐡 𝐓𝐨𝐦𝐦𝐲 || a Tommyinnit playlist ♡"},"description":{"simpleText":"!! ART IS BY ULTIMATEDIRK ON TUMBLR, HOWEVER THEY DO NOT WANT TO BE ASSOCIATED WITH DSMP ANY LONGER AND HAVE SINCE DELETED THE POST. HERE IS A REBLOG: https://b-e-e-e-s.tumblr.com/post/630043986183192576/first-drawing-with-my-laptop-fixed-have-a !!\na tommy playlist to hold you guys over!! my wilbur one went haywire and i felt a lil discouraged but hopefully this one will be okay :D !!"},"lengthSeconds":"3623","ownerProfileUrl":"http://www.youtube.com/channel/UC_ptvA3rNT5FmAqG_06QHlg","
                */
            }

            string[] outputBlocks = { block1, block2, block3 };

            return outputBlocks;
        }
    }
}
