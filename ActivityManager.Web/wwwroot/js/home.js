/*let listitemTemplate = document.getElementById("listitemTemplate");

const form = document.querySelector('form');
const activityNameInput = document.querySelector("[name='activityNameInput']");
const activitiesList = document.getElementById('activities-list');

let currentDeleteID = 0;

// Side Effects / Lifecycle

const existingActivitiesJson = JSON.parse(localStorage.getItem('activities')) || [];

const activitiesArray = [];

/*existingActivitiesJson.forEach(activtiy => {
    addTodo(activtiy);
});*/
/*
function addTodo(activtiyName) {
    activitiesArray.push(activtiyName);
    const clone = listitemTemplate.content.cloneNode(true);
    //clone.dataset.activityName = activtiyName; TODO
    let a = clone.getElementById('nameLink');
    a.innerHTML = activtiyName;
    activitiesList.appendChild(clone);
    localStorage.setItem('activities', JSON.stringify(activitiesArray));
    activityNameInput.value = ''
}*/

/*function clickDeleteActivity() {
    let text = "Press a button!\nEither OK or Cancel.";
    if (confirm(text) == true) {
        //todo
    } 
    // currentDeletID = button parent dataset
    // show "are u sure" panel
}

// Events
form.onsubmit = (event) => {
    event.preventDefault();
    addTodo(activityNameInput.value);
}*/

 