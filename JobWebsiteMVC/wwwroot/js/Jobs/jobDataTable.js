$(function () {
    console.log("jobDataTable loaded");

    var table = new DataTable('#tblJobs', {
        ajax: {
            url: '../api/Jobs/GetMyJobs',
            dataSrc: '',
        },
        columns: [
            { data: 'jobTitle' },
            {
                data: 'description',
                render: function (data, type) {
                    if (type === 'display') {
                        if (data.length > 100) {
                            data = data.substring(0, 100) + "...";
                            return data;
                        }
                    }
                    return data;
                }
            },
            {
                data: 'isDraft',
                className: "text-center",
                render: function (data, type) {
                    if (type === 'display') {
                        if (data === true) {
                            return '<span class="material-icons text-success"> done </span>';
                        } else {
                            return '<span class="material-icons text-danger"> close </span>'
                        }
                        return data;
                    }
                    return data;
                }
            },
            {
                data: 'minSalary',
                render: function (data, type) {
                    var number = DataTable.render.number(',', '.', 2, '£').display(data);
                    return number;
                }
            },
            {
                data: 'maxSalary',
                render: function (data, type) {
                    var number = DataTable.render.number(',', '.', 2, '£').display(data);
                    return number;
                }
            },
            { data: 'hoursPerWeek' },
            {
                data: 'holidayEntitlement',
                render: function (data, type) {
                    var number = DataTable.render.number(',', '.', 2,).display(data);
                    return number;
                }
            },
            { data: 'jobType.description' },
            {
                data: 'isActive',
                className: "text-center",
                render: function (data, type) {
                    if (type === 'display') {
                        if (data === true) {
                            return '<span class="material-icons text-success"> done </span>';
                        } else {
                            return '<span class="material-icons text-danger"> close </span>'
                        }
                        return data;
                    }
                    return data;
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    return '<div class="btn-group" role="group">' +
                        '<button type="button" class="btn btn-primary view" id=v-"' + meta.row + '"><span class="material-icons">info</span></button>' +
                        '<button type="button" class="btn btn-info edit" id=e-"' + meta.row + '"><span class="material-icons">edit</span></button>' +
                        '<button type="button" class="btn btn-danger delete" id=d-"' + meta.row + '"><span class="material-icons">delete</span></button>' +
                        '</div>'
                }
            }
        ],
        processing: true,
    });

    table.on('click', '.view', function (e) {
        var id = $(this).attr("id").match(/\d+/)[0];
        let data = table.row(e.target.closest('tr')).data();

        alert(data.id + " View Clicked. Job Title is " + data.jobTitle);
    });
    table.on('click', '.edit', function (e) {
        var id = $(this).attr("id").match(/\d+/)[0];
        let data = table.row(e.target.closest('tr')).data();

        alert(data.id + " Edit Clicked. Job Title is " + data.jobTitle);
    });
    table.on('click', '.delete', function (e) {
        var id = $(this).attr("id").match(/\d+/)[0];
        let data = table.row(e.target.closest('tr')).data();

        alert(data.id + " Delete Clicked. Job Title is " + data.jobTitle);
    });
});