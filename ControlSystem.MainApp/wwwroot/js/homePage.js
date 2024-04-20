function openFullInfo(
        description, topic, 
        dateString, version) {

        $(".modalInfo").find("#patch-version").text('Обновление: ' + version);
        $(".modalInfo").find("#patch-date").text(dateString);

        topic = topic.replace(/\n/g, "<br>");

        $(".modalInfo").find("#patch-topic").html(topic);

        description = description.replace(/\n/g, "<br>")
                                 .replace(/\t/g, "&nbsp;&nbsp;&nbsp;&nbsp;");

        $(".modalInfo").find("#patch-description").html(description);

        document.getElementById('full-info').style.display = 'block';
        document.getElementById('overlay').style.display = 'block';
    }

    function closeFullInfo() {
        document.getElementById('full-info').style.display = 'none';
        document.getElementById('overlay').style.display = 'none';
    }

    $(".topic").each(function() {
        
        var labelText = $(this).text();
        
        labelText = labelText.replace(/\n/g, "<br>");
        
        $(this).html(labelText);
    });