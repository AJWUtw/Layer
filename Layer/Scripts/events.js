$(document).ready(function () {
    $("#searchButton").kendoButton({
        click: onClick
    });

    function onClick(e) {

        $.post("/Order/GetOrderByCondition", $("#searchOrderCondition").serialize(),
           function (data) {
               console.log(data);
               console.log(viewModel); //  2pm
           }, "json");
        //kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");

        //console.log(e);
        //xhr.post({
        //    url: "/Order/GetOrderByCondition",
        //    form: dom.byId("searchOrderCondition"),
        //    handleAs: "json",
        //    load: function (jsonData) {
        //        store = new ItemFileWriteStore({ data: jsonData });
        //        ready(function () {
        //            orderStore.close();
        //            orderGrid.setStore(store);
        //            orderGrid._refresh();
        //            console.log('Success');
        //        });
        //    },
        //    error: function (e) {
        //        console.log(e);
        //    }
        //});
    }
});