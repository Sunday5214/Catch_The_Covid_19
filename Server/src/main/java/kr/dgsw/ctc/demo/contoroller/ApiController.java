package kr.dgsw.ctc.demo.contoroller;

import io.swagger.annotations.ApiOperation;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.view.RedirectView;

import javax.servlet.http.HttpServletResponse;
import java.sql.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@CrossOrigin(origins = "*")

@Controller
public class ApiController {

    @RequestMapping(path = "/home")
    public String main(){
        return "index";
    }

    @ApiOperation(value = "insertRecord", notes = "측정값 저장.")
    @ResponseBody
    @RequestMapping(path = "/insertRecord", method = RequestMethod.GET)
    public Map<String, String> insertRecord(@RequestParam String Idx, @RequestParam String code, @RequestParam String temp) {
        Map<String, String> map = new HashMap<>();

        String sql = String.format("INSERT INTO record (`id`, recordTime,`recordCode`,`temp`) VALUES ('%s',(now()),'%s','%s');", Idx.trim(), code.trim(), temp.trim());
        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            Statement st = con.createStatement();
            st.executeUpdate(sql);

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        map.put("code", "200");
        return map;
    }

    @ApiOperation(value = "searchCard", notes = "카드값 신원 조회.")
    @ResponseBody
    @RequestMapping(path = "/searchCard", method = RequestMethod.GET)
    public Map<String, String> searchCard(@RequestParam String cardId, HttpServletResponse response) {

        Map<String, String> map = new HashMap<>();
        int cnt = 0;

        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            String qu = "select * from card where cardId=\'" + cardId + "\'";
            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(qu);

            while (rs.next()) {
                map.put("id", String.valueOf(rs.getInt("Idx")));
                map.put("grade", String.valueOf(rs.getInt("grade")));
                map.put("class", String.valueOf(rs.getInt("class")));
                map.put("classNumber", String.valueOf(rs.getInt("number")));
                map.put("name", rs.getString("name"));
                map.put("student", String.valueOf(rs.getBoolean("student")));
                map.put("cardId", rs.getString("cardId"));
                cnt++;
            }
            st.close();

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        if (cnt == 0)
            response.setStatus(HttpServletResponse.SC_NOT_FOUND);
        else
            response.setStatus(HttpServletResponse.SC_OK);
        return map;
    }

    @ApiOperation(value = "addCard", notes = "카드 등록.")
    @ResponseBody
    @RequestMapping(path = "/addCard", method = RequestMethod.GET)
    public Map<String, String> addCard(@RequestParam String grade, @RequestParam String class_, @RequestParam String number, @RequestParam String name, @RequestParam String student, @RequestParam String cardId) {
        Map<String, String> map = new HashMap<>();
        String sql = String.format("INSERT INTO card (`grade`, `class`, `number`, `name`, `student`, `cardId`) VALUES ('%s', '%s', '%s', '%s', '%s', '%s');", grade, class_, number, name, student, cardId);
        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            Statement st = con.createStatement();
            st.executeUpdate(sql);

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        return map;
    }

    @ApiOperation(value = "allUser", notes = "모든 유저 조회.")
    @ResponseBody
    @RequestMapping(path = "/allUser", method = RequestMethod.GET)
    public List allUser(HttpServletResponse response) {

        int cnt = 0;
        List json = new ArrayList();

        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            String qu = "select * from card order by card.grade, card.class, card.number";
            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(qu);

            while (rs.next()) {
                Map<String, String> map = new HashMap<>();

                map.put("id", String.valueOf(rs.getInt("Idx")));
                map.put("grade", String.valueOf(rs.getInt("grade")));
                map.put("class", String.valueOf(rs.getInt("class")));
                map.put("classNumber", String.valueOf(rs.getInt("number")));
                map.put("name", rs.getString("name"));
                map.put("student", String.valueOf(rs.getBoolean("student")));
                map.put("cardId", rs.getString("cardId"));

                json.add(map);
                cnt++;
            }
            st.close();

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        if (cnt == 0)
            response.setStatus(HttpServletResponse.SC_NOT_FOUND);
        else
            response.setStatus(HttpServletResponse.SC_OK);

        return json;
    }

    @ApiOperation(value = "info", notes = "학년, 반 개수 및 측정시간코드 조회.")
    @RequestMapping(path = "/info", method = RequestMethod.GET)
    public @ResponseBody
    Map<String, Object> info() {

        Map<String, Object> map = new HashMap<>();
        List<String> list = new ArrayList();

        map.put("grade", "3");
        map.put("class", "3");
        map.put("check", "15"); //열 체크 한 학생 수
        map.put("uncheck", "5"); //열 체크를 안한 학생 수
        map.put("student_cnt", "180"); //열 체크 한 학생 수
        // (전체 학생 수, 열 체크를 안한 학생 수) 또는 (전체 학생 수, 열 체크를 한 학생 수)로 넘겨주면 될 것 같습니단~

        list.add("아침");
        list.add("점심");
        list.add("저녁");
        list.add("입실");
        list.add("퇴실");

        map.put("codes", list);

        return map;
    }

    @ApiOperation(value = "searchRecord", notes = "측정 기록 조회.")
    @ResponseBody
    @RequestMapping(path = "/searchRecord", method = RequestMethod.GET)
    public List searchRecord(HttpServletResponse response, @RequestParam String code) {

        int cnt = 0;
        List json = new ArrayList();

        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            String qu = "select * from record where recordCode=" + code + " order by card.grade, card.class, card.number";
            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(qu);

            while (rs.next()) {
                Map<String, String> map = new HashMap<>();

                map.put("Idx", String.valueOf(rs.getInt("Idx")));
                map.put("id", String.valueOf(rs.getInt("id")));
                map.put("recordTime", rs.getDate("recordTime").toString());
                map.put("recordCode", String.valueOf(rs.getInt("recordCode")));
                map.put("temp", String.format("%.2f",rs.getFloat("temp")));

                json.add(map);
                cnt++;
            }
            st.close();

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        if (cnt == 0)
            response.setStatus(HttpServletResponse.SC_NOT_FOUND);
        else
            response.setStatus(HttpServletResponse.SC_OK);

        return json;
    }

    @ApiOperation(value = "searchRecordFilter", notes = "측정 기록 조회(필터)")
    @ResponseBody
    @RequestMapping(path = "/searchRecordFilter", method = RequestMethod.GET)
    public List searchRecordFilter(HttpServletResponse response, @RequestParam String grade, @RequestParam String class_,@RequestParam String date, @RequestParam String number,@RequestParam String recordCode) {
//,@RequestParam String date
        int cnt = 0;
        List json = new ArrayList();

        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8", "root", "root");
            String qu = String.format("select card.grade, card.class, card.number, card.name, record.temp, record.recordCode, card.student, card.cardId from record Join card ON record.id = card.Idx");

            if (!date.equals("0")) {
                qu += String.format(" AND DATE(record.recordTime)=\'%s\'", date);
            }
            if (!grade.equals("0")) {
                qu += String.format(" AND card.grade=%s", grade);
            }
            if (!class_.equals("0")) {
                qu += String.format(" AND card.class=%s", class_);
            }
            if (!number.equals("0")) {
                qu += String.format(" AND card.number=%s", number);
            }
            if (!recordCode.equals("0")) {
                qu += String.format(" AND record.recordCode=%s", recordCode);
            }

            qu += " order by card.grade, card.class, card.number";

            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(qu);

            while (rs.next()) {
                Map<String, String> map = new HashMap<>();
                map.put("grade", String.valueOf(rs.getInt("grade")));
                map.put("class", String.valueOf(rs.getInt("class")));
                map.put("number", String.valueOf(rs.getInt("number")));
                map.put("name", rs.getString("name"));
                map.put("temp", String.format("%.2f",rs.getFloat("temp")));
                map.put("recordCode", String.valueOf(rs.getInt("recordCode")));
                map.put("student", String.valueOf(rs.getBoolean("student")));
                map.put("cardId", rs.getString("cardId"));
                json.add(map);
                cnt++;
            }
            st.close();

        } catch (Exception e) {
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        return json;
    }


    @GetMapping("/swagger")
    public RedirectView index() {
        return new RedirectView("/swagger-ui.html");
    }


}
