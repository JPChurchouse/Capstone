// send the page title as a chrome message
//chrome.runtime.sendMessage(document.documentElement.innerHTML);
chrome.runtime.sendMessage({htmlmsg : true, scrapped : document.title});
//txt = document.documentElement.innerHTML;