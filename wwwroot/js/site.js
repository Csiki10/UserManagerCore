$(document).ready(function () {

    var searchInput = $('.grid-search');

    searchInput.on('keyup', function () {
        var query = $(this).val();
        console.log(query)

        if (query.length >= 3 || query.length === 0) {

            var grid = $("#usersGrid").data("kendoGrid");
            grid.dataSource.filter({
                logic: "or",
                filters: [
                    { field: "Username", operator: "contains", value: query },
                    { field: "LastName", operator: "contains", value: query },
                    { field: "FirstName", operator: "contains", value: query },
                    { field: "PlaceOfBirth", operator: "contains", value: query },
                    { field: "PlaceOfResidence", operator: "contains", value: query },
                    //{ field: "DateOfBirth", operator: "eq", value: query }
                ]
            });
        }
    });

    $("#saveToXmlBtn").on("click", function () {
        $.ajax({
            url: '@Url.Action("SaveToXml", "Users")',
            type: 'POST',
            success: function () {
                alert("Data saved to XML successfully.");
            },
            error: function () {
                alert("Error saving data to XML.");
            }
        });
    });
});