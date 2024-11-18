var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
    
});

function loadDataTable(status) {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [

            { "data": "order_id", "width": "5%" },
            { "data": "appUser.user_name", "width": "15%" },
            { "data": "appUser.phoneNumber", "width": "20%" },
            { "data": "appUser.email", "width": "20%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "order_id",
                "render": function (data) {
                    return `
                     
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Details?order_id=${data}" 
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                   
                  
                     </div>
                `
                },
                "width": "10%"
            },
        ]

    });
}

