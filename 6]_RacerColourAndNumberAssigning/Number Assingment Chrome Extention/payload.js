// send the page title as a chrome message
//chrome.runtime.sendMessage(document.documentElement.innerHTML);

//chrome.tabs.onUpdated.addListener(function(tabId, changeInfo, tab) {
 //   chrome.runtime.sendMessage({htmlmsg : true, scrapped : document.title});
//});


chrome.runtime.sendMessage({htmlmsg : true, scrapped : document.title});

//txt = document.documentElement.innerHTML;

//chrome.runtime.onMessage.addListener(function (message){
//	if(message.contentreq){
//        chrome.runtime.sendMessage({htmlmsg : true, scrapped : document.title});
//    }
//});