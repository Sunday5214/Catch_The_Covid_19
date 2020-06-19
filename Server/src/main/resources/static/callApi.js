let today = "2020-06-08";
let student_cnt = "0"; //전체 학생 수
let check_cnt = "0"; //체크된 학생 수
let timeList = []; //시간 리스트
const BASICTEMP = 37.49; //기준온도
const SERVEIP = "10.80.162.7:8080"; //192.168.43.16:8080

$(document).ready(function () {
    //학년,반,타임 정보를 받아와 그려준다.
    $.ajax({
        type: "GET",
        url: 'http://' + SERVEIP + '/info',
        dataType: 'json',
        success: function (response) {
            console.log("학교정보를 읽었습니다.");
            console.log(response);

            //학년
            for (let i = 1; i <= response.grade; i++) {
                $("#id_grade").append(
                    "<option value=" + i + ">" + i + "학년</option>"
                );
            };
            //학반
            for (let i = 1; i <= response.class; i++) {
                $("#id_class").append(
                    "<option value=" + i + ">" + i + "반</option>"
                );
            };
            //타임 정보
            for (let i = 0; i < response.codes.length; i++) {
                $("#id_time").append(
                    "<option value=" + (i + 1) + ">" + response.codes[i] + "</option>"
                );
            };

            timeList = response.codes // 시간 설정
            //전체 학생 수 설정
            student_cnt = response.student_cnt;
            //console.log("student_cnt : "+response.student_cnt+"timeList : "+timeList);
        }
    })
    //select Box 날짜 설정
    todayDate();
    //도넛 그래프 재로드
    $('.js-chart').setChart();
    $('.js-count').count();
});

// 필터에 따른 정보를 받아온다
$("#filter").click(function () {
    let SelectGrade = $("#id_grade").val();
    let SelectClass = $("#id_class").val();
    let SelectTime = $("#id_time").val();
    let SelectDate = $("#SelectDate").val();
    today = SelectDate;
    console.log(SelectGrade + SelectClass + SelectTime + ":" + SelectDate);

    if (SelectTime == 0) {
        alert("시간을 선택해주세요.");
    } else {
        // 필터에 따른 정보를 모두 받아온다
        $.ajax({
            type: "GET",
            url: 'http://'+ SERVEIP +'/searchRecordFilter?class_=' + SelectClass + '&date=' + SelectDate + '&grade=' + SelectGrade + '&number=0&recordCode=' + SelectTime,
            dataType: 'json',
            async: true,
            success: function (response) {
                console.log("필터 목록을 읽었습니다.");
                console.log(response);

                //테이블 초기화
                $(".studentSection").remove();

                //검색된 결과가 없다면
                if(response.length == 0){
                    console.log("NULLLLLLLLL");
                    $(".tablebody").append(
                        "<div class='studentSection Non'>" +
                        "조회된 데이터가 없습니다." +
                        "</div>"
                    )
                }else{
                    setList(response);
                }

                //도넛 그래프 재로드
                student_cnt = student_cnt;
                check_cnt = response.length;
                $('.js-chart').setChart();
                $('.js-count').count();
            },
            error: function (state, error) {
                console.log("필터 목록 로드 에러", state, error);
                alert("필터 목록 로드 에러");
            }
        });
    }

});

//Click한 학생의 카드키 획득!!!!
$(document).on("click", '.studentSection', function () {
    console.log("checking...cardId");
    let SelectTime = $("#id_time").val();
    let tmp = $(this).attr("name");
    let nameclass = $(this).children().attr("id");
    //console.log(nameclass);

    $.ajax({
        type: "GET",
        url: 'http://' + SERVEIP + '/searchCard?cardId=' + nameclass,
        dataType: 'json',
        async: true,
        success: function (response) {
            console.log("학생정보를 읽었습니다.");
            console.log(response);
            //학생카드 초기화 해야한다.
            $(".stdentId").remove();

            $("#temp").append

            $(".codeNum").append(
                "<div class='stdentId'>" +
                response.cardId + "</div>"
            );

            $(".student-info").append(
                "<div class='stdentId'> 이름 | " +
                response.name + "<br>" +
                "학년 | " + response.grade + "<br>" +
                "학반 | " + response.class + "<br>" +
                "번호 | " + response.classNumber + "<br>" +
                today + "_" + timeList[SelectTime-1] + 
                " 기록 <br> <span id='temp'>" + tmp + " 도</span>" +
                "</div>"
            );
        },
        error: function (state, error) {
            console.log("checking..cardId..Error", state, error);
            alert("학생 정보 로드 에러");
        }
    });
});

//select Box 날짜 설정
function todayDate() {
    var date = new Date();
    today = date.getFullYear() + "-" + ("0" + (date.getMonth() + 1)).slice(-2) + "-" + ("0" + date.getDate()).slice(-2);
    console.log(today);
    $("#SelectDate").attr('value',today);
    $("#SelectDate").attr('max',today);
}

//검색되 리스트 렌더링
function setList(response) {
    for (let i = 0; i < response.length; i++) {
        $(".tablebody").append(
            "<div class='studentSection' id='" + i + "' name='" + response[i].temp +"'>" +
            "<div class='cardId' id='" + response[i].cardId + "'></div>" +
            "<div class='section date'>" + today + "</div>" +
            "<div class='section grade'>" + response[i].grade + "</div>" +
            "<div class='section class'>" + response[i].class + "</div>" +
            "<div class='section number'>" + ("0" + response[i].number).slice(-2) + "</div>" +
            "<div class='section name'>" + response[i].name + "</div>" +
            "<div class='section temp'>" + response[i].temp + "</div>" +
            "</div>"
        );
        // 1. 37.5도 보다 높으면 .studentSection over 로 클래스를 바꾼다.
        // 2. 37.5도 보다 높으면 i를 id로 가지는 studentSection 클래스에 over을 추가한다.
        if(response[i].temp > BASICTEMP){
            $("#"+ i).attr("class","studentSection over");
        }
    }
}