let skip = 6;
let coursesCount = $("#loadMore").next().val();
$(document).on('scroll', '#loadMore', function () {
    $.ajax({
        url: "/Courses/LoadMore/",
        type: "get",
        data: {
            "skipCount": skip
        },
        success: function (res) {
            $("#myCourses").append(res)
            skip += 6;

            if (coursesCount <= skip) {
                $("#loadMore").remove()
            }

        }
    });
});
