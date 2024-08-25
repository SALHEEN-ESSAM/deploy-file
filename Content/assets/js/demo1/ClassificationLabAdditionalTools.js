$(function () {
   
    $("#LAB3").click(function () {
        //debugger;
        var isValid = true;
        //var itemsList = [];
        var factID = $('#FactoryId').val();
        var Cy = parseInt($('.Cylinder').val());
        var Cu = $('.Cubles').val();
        var Year = $('#ClsYear').val();
           
            var item = {
                FactoryId:factID,
                Cylinder:Cy,
                Cubles:Cu,
                ClsYear:Year,
            }
          
            //itemsList.push(item);
 
        if (isValid) {
            var data = item

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabAddtion/SaveLabAdditionalTools',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);

                    }
                    else {
                        $('#Ordermessages').text(data.message);

                    }

                }

            });

        }




    });
});








