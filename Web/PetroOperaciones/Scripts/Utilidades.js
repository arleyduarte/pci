

$(document).ready(function () {

    var date = new Date();
    date.setDate(date.getDate() + 5);

    $("table tr:nth-child(even)").addClass("striped");



    $(document).ready(function () {
        var d = new Date();
        var curr_date = d.getDate();
        var curr_month = d.getMonth();
        curr_month++;
        var curr_year = d.getFullYear();
        var dToday = curr_month + "/" + curr_date + "/" + curr_year;



        if ($('#FechaCumpleanos').val() == "" && $('#FechaCumpleanos').text() == "") { $('#FechaCumpleanos').attr('value', dToday); }
        $('#FechaCumpleanos').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaFinalizacion').val() == "" && $('#FechaFinalizacion').text() == "") { $('#FechaFinalizacion').attr('value', dToday); }
        $('#FechaFinalizacion').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaCreacion').val() == "" && $('#FechaCreacion').text() == "") { $('#FechaCreacion').attr('value', dToday); }
        $('#FechaCreacion').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaFinal').val() == "" && $('#FechaFinal').text()=="") { $('#FechaFinal').attr('value', dToday); }
        $('#FechaFinal').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaInicial').val() == "") { $('#FechaInicial').attr('value', dToday); }
        $('#FechaInicial').datepicker({ format: 'mm/dd/yyyy' });




        if ($('#Filtro_FechaInicial').val() == "") { $('#Filtro_FechaInicial').attr('value', dToday); }
        $('#Filtro_FechaInicial').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#Filtro_FechaFinal').val() == "") { $('#Filtro_FechaFinal').attr('value', dToday); }
        $('#Filtro_FechaFinal').datepicker({ format: 'mm/dd/yyyy' });


        if ($('#FechaFactura').val() == "") { $('#FechaFactura').attr('value', dToday); }
        $('#FechaFactura').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaResolucion').val() == "") { $('#FechaResolucion').attr('value', dToday); }
        $('#FechaResolucion').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaCotizacion').val() == "" && $('#FechaCotizacion').text() == "") { $('#FechaCotizacion').attr('value', dToday); }
        $('#FechaCotizacion').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaVigencia').val() == "" && $('#FechaVigencia').text() == "") { $('#FechaVigencia').attr('value', dToday); }
        $('#FechaVigencia').datepicker({ format: 'mm/dd/yyyy' });

        if ($('#FechaAnticipo').val() == "" && $('#FechaAnticipo').text() == "") { $('#FechaAnticipo').attr('value', dToday); }
        $('#FechaAnticipo').datepicker({ format: 'mm/dd/yyyy' });


        if ($('#FechaVencimiento').val() == "" && $('#FechaVencimiento').text() == "") { $('#FechaVencimiento').attr('value', dToday); }
        $('#FechaVencimiento').datepicker({ format: 'mm/dd/yyyy' });
        

    });


})


