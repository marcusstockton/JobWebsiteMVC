$(function () {

    $.get("Home/JobsCreatedInLastMonth", function (res) {
        var ctx = document.getElementById('myChart').getContext('2d');
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: $.map(res, function (i, v) { return i.item1 }),
                datasets: [{
                    label: '# of Jobs Created Last Month',
                    fill: false,
                    tension: 0.1,
                    data: $.map(res, function (i, v) { return i.item2 }),
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(152, 136, 62, 0.2)',
                        'rgba(218, 174, 16, 0.2)',
                        'rgba(108, 182, 47, 0.2)',
                        'rgba(51, 28, 181, 0.2)',
                        'rgba(214, 111, 102, 0.2)',
                        'rgba(32, 96, 223, 0.2)',
                        'rgba(50, 239, 245, 0.2)',
                        'rgba(243, 141, 18, 0.2)',
                        'rgba(166, 167, 221, 0.2)',
                        'rgba(233, 216, 233, 0.2)',
                        'rgba(185, 238, 27, 0.2)',
                        'rgba(224, 68, 6, 0.2)',
                        'rgba(20, 66, 0, 0.2)',
                        'rgba(151, 232, 38, 0.2)',
                        'rgba(51, 58, 255, 0.2)',
                        'rgba(213, 226, 116, 0.2)',
                        'rgba(243, 43, 149, 0.2)',
                        'rgba(213, 21, 21, 0.2)',
                        'rgba(165, 39, 39, 0.2)',
                        'rgba(183, 68, 36, 0.2)',
                        'rgba(77, 168, 52, 0.2)',
                        'rgba(52, 150, 168, 0.2)',
                        'rgba(88, 0, 219, 0.2)',
                        'rgba(0, 219, 88, 0.2)',
                        'rgba(204, 255, 122, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)',
                        'rgba(152, 136, 62, 1)',
                        'rgba(218, 174, 16, 1)',
                        'rgba(108, 182, 47, 1)',
                        'rgba(51, 28, 181, 1)',
                        'rgba(214, 111, 102, 1)',
                        'rgba(32, 96, 223, 1)',
                        'rgba(50, 239, 245, 1)',
                        'rgba(243, 141, 18, 1)',
                        'rgba(166, 167, 221, 1)',
                        'rgba(233, 216, 233, 1)',
                        'rgba(185, 238, 27, 1)',
                        'rgba(224, 68, 6, 1)',
                        'rgba(20, 66, 0, 1)',
                        'rgba(151, 232, 38, 1)',
                        'rgba(51, 58, 255, 1)',
                        'rgba(213, 226, 116, 1)',
                        'rgba(243, 43, 149, 1)',
                        'rgba(213, 21, 21, 1)',
                        'rgba(165, 39, 39, 1)',
                        'rgba(183, 68, 36, 1)',
                        'rgba(77, 168, 52, 1)',
                        'rgba(52, 150, 168, 1)',
                        'rgba(88, 0, 219, 1)',
                        'rgba(0, 219, 88, 1)',
                        'rgba(204, 255, 122, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        stepSize: 1
                    }
                }
            }
        });
    });

});