﻿function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    toastr.success(data.message);

                    // Table rows have the same id as the data in them, with a "row_" prefix
                    // This is used for deleting the row directly from here to avoid having to refresh the page
                    var deletedRow = document.getElementById('row_' + data.deletedId);
                    if (deletedRow)
                        deletedRow.parentNode.removeChild(deletedRow);
                }
            })
        }
    });
}

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