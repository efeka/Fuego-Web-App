function truncateDescription() {
    var descriptionCells = document.getElementsByClassName('descriptionCell');
    var maxLength = 15;

    for (var i = 0; i < descriptionCells.length; i++) {
        var descriptionCell = descriptionCells[i];
        var description = descriptionCell.innerText;

        if (description.length > maxLength) {
            descriptionCell.innerText = description.substring(0, maxLength) + '...';
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    truncateDescription();
});