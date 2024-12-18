﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [

            { "data": "pro_name", "width": "25%" },
            { "data": "pro_description", "width": "15%" },
            { "data": "author", "width": "10%" },
            { "data": "listPrice", "width": "10%" },
            { "data": "category.book_Name", "width": "10%" },
            { "data": "coverType.cover_Name", "width": "10%" },
            {
                "data": "pro_id",
                "render": function (data) {
                    return `
                     
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Product/Upsert?pro_id=${data}" 
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                   
                   <a OnClick=Delete('/Admin/Product/Delete/${data}') 
                    class="btn btn-danger mx-2" ><i class="bi bi-trash"></i>
                        </a>
                     </div>
                `
                },
                "width": "30%"
            },
        ]

    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}