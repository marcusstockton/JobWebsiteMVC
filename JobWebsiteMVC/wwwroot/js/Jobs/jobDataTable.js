$(function () {
    console.log("jobDataTable loaded");

    new DataTable('#tblStudent', {
        ajax: {
            url: '../api/Jobs/',
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
                            console.log("True");
                            return '<span class="material-icons text-success"> done </span>';

                        } else {
                            console.log("False");
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
                render: function(data, type){
                    if (type === 'display') {
                        if (data === true) {
                            console.log("True");
                            return '<span class="material-icons text-success"> done </span>';

                        } else {
                            console.log("False");
                            return '<span class="material-icons text-danger"> close </span>'
                        }
                        return data;
                    }
                    return data;
                }
            },
        ],
        processing: true,
    });
});