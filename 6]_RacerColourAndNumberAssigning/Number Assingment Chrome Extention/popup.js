// Inject the payload.js script into the current tab
//chrome.extension.getBackgroundPage().chrome.tabs.executeScript(null, {
//	file: 'payload.js'
//});;

//if(document.getElementById('auto').checked){
//	chrome.extension.getBackgroundPage().chrome.tabs.executeScript(null, {
//		file: 'AutoButton.js'
//	});;
//}

window.addEventListener('load', function (evt) {
	//Connect to port
	chrome.extension.getBackgroundPage().chrome.tabs.executeScript(null, {
		file: 'payload.js'
	});;
});


// Listen to messages from the payload.js script and write to popout.html
chrome.runtime.onMessage.addListener(function (message){
	//alert("Click Detected");
	if(message.htmlmsg){
		if(document.getElementById('auto').checked)
		{
			console.log("Message Sent");
			//Grab Numbers
			//Put in Text Boxes
			//Save Numbers
			document.getElementById('pagetitle').innerHTML = message.scrapped;
		}
	}
});

//Save
document.getElementById('port').addEventListener('change',savePort);
function savePort(){
	var Num = document.getElementById('port').value;
	chrome.storage.sync.set({
	  PortNum: Num,
	});
}

document.getElementById('auto').addEventListener('click',saveAuto);
function saveAuto(){
	var Mode = document.getElementById('auto').checked;
	chrome.storage.sync.set({
	  AutoModeSet: Mode,
	});
}

document.getElementById('blue').addEventListener('click',saveblue);
function saveblue(){
	var Mode = document.getElementById('blue').checked;
	chrome.storage.sync.set({
	  BlueSet: Mode,
	});
}

document.getElementById('green').addEventListener('click',savegreen);
function savegreen(){
	var Mode = document.getElementById('green').checked;
	chrome.storage.sync.set({
	  GreenSet: Mode,
	});
}

document.getElementById('orange').addEventListener('click',saveorange);
function saveorange(){
	var Mode = document.getElementById('orange').checked;
	chrome.storage.sync.set({
	  OrangeSet: Mode,
	});
}

//Start Race
document.getElementById('start').addEventListener('click',startrace)
function startrace(){
	consoleID = document.getElementById("consoleID");
	consoleID.innerHTML = "Race Started!"
	var racejson = {
		"KartList" : [],
		"Laps" : []
	}

	var checkedboxes = document.getElementById("karts").querySelectorAll('input[type=checkbox]')
	var kartcolours = document.getElementById("karts").querySelectorAll('input[type=checkbox]')
	var kartnumbers = document.getElementById("karts").querySelectorAll('input[type=number]')
	
	for(var i = 0; i < checkedboxes.length; i++){
		if(checkedboxes[i].checked){
			addracer(racejson,kartcolours[i].id,kartnumbers[i].value)
		}
	}
	console.log(racejson);

}
function addracer(raceinfo, newcolour, newnumber){
	var jsonData = {};
	var colourcol = "Colour";
	jsonData[colourcol] = newcolour;

	var numbercol = "Number";
	jsonData[numbercol] = newnumber;

    raceinfo.KartList.push(jsonData);
}

//select all checkboxes
document.getElementById('select').addEventListener('click',selectallcolours)
function selectallcolours(){
	var boxes = document.getElementById("karts").querySelectorAll('input[type=checkbox]')
	for (var i = 0; i < boxes.length; i++) {
		document.getElementById(boxes[i].id).checked = true
	}
}

//Deselect all checkboxes
document.getElementById('deselect').addEventListener('click',deselectallcolours)
function deselectallcolours(){
	var boxes = document.getElementById("karts").querySelectorAll('input[type=checkbox]')
	for (var i = 0; i < boxes.length; i++) {
		document.getElementById(boxes[i].id).checked = false
	}
}

document.getElementById('cancel').addEventListener('click',cancelrace)
function cancelrace(){
	console = document.getElementById("consoleID");
	console.innerHTML = "Race Cancelled!"
	//send out cancel command
}



document.addEventListener('DOMContentLoaded', restore_options);
function restore_options(){
	chrome.storage.sync.get({
		//Add all boxes here
		PortNum: '1810',
		AutoModeSet: true,
		GreenSet: true,
		BlueSet: true,
		OrangeSet: true,
		//Title: "Dummy"
	  }, function(items) {
		document.getElementById('port').value = items.PortNum;
		document.getElementById('auto').checked = items.AutoModeSet;
		document.getElementById('green').checked = items.GreenSet;
		document.getElementById('blue').checked = items.BlueSet;
		document.getElementById('orange').checked = items.OrangeSet;
		//document.getElementById('pagetitle').innerHTML = items.Title;
	  });
}

//document.getElementById('auto').addEventListener('click',checkAuto);
//function checkAuto(){
//	var autobox = document.getElementById('auto');
//  if (autobox.checked = true) {
//    console.log("checked");
//  } else {
//    console.log("not checked");
//  }
//};
//	var button = document.createElement("button");
//	document.body.appendChild(button);


//navigator.clipboard.writeText("MY PLAIN TEXT")
  //                                 .then(() => { alert("Copy successful"); })
    //                               .catch((error) => { alert(`Copy failed! ${error}`); });