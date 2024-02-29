var popup = document.getElementById('popup');

function openPopup() {
    popup.style.display = "block";
    document.body.style.overflowY = "hidden";
}

function closePopup() {
    popup.style.display = "none";
    document.body.style.overflowY = "auto";
}
