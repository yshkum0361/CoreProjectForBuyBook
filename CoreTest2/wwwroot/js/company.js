﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [

            { "data": "comp_name", "width": "15%" },
            { "data": "comp_StAddress", "width": "25%" },
            { "data": "comp_City", "width": "10%" },
            { "data": "comp_State", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "comp_Id",
                "render": function (data) {
                    return `
                     
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Company/Upsert?comp_id=${data}" 
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                   
                   <a OnClick=Delete('/Admin/Company/Delete/${data}') 
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