// Chart Functionality
$.fn.setChart = function () {
   return this.each(function () {
      // Variables
      var chart = $(this),
         path = $('.chart__foreground path', chart),
         dashoffset = path.get(0).getTotalLength(),
         goal = student_cnt, //전체 학생 수
         consumed = check_cnt; //체크된 학생 수
         
      chart.attr('data-goal','goal');
      chart.attr('data-count','consumed');

      //Console Test
      // console.log("====" + goal + "====");
      // console.log("====" + consumed + "====");

      percentage = consumed / goal * 100;
      percentage = parseInt(percentage);
      document.getElementById('percent').innerHTML = percentage;
      //확인 (체크된 학생 수)
      document.getElementById('count_consumed').innerHTML = consumed;
      //미확인 (전체 학생 수 - 체크된 학생 수)
      document.getElementById('count_remained').innerHTML = goal - consumed;

      $('.chart__foreground', chart).css({
         'stroke-dashoffset': Math.round(dashoffset - ((dashoffset / goal) * consumed))
      });
   });
}; // setChart()

// Count
$.fn.count = function () {
   return this.each(function () {
      $(this).prop('Counter', 0).animate({
         Counter: $(this).attr('data-count')
      }, {
         duration: 1000,
         easing: 'swing',
         step: function (now) {
            $(this).text(Math.ceil(now));
         }
      });
   });
}; // count()

