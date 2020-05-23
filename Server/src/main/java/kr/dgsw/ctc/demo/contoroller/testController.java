package kr.dgsw.ctc.demo.contoroller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.sql.*;
import java.util.HashMap;
import java.util.Map;

@Controller
public class testController {

    @RequestMapping(value = "/searchCard")
    public @ResponseBody
    String home(){
        return "index";
    }

    @ResponseBody
    @GetMapping("/insertRecord")
    public Map<String, String> insertRecord(@RequestParam String Idx,@RequestParam String temp){
        Map<String, String> map = new HashMap<>();
        String sql = String.format("INSERT INTO record (`id`, recordTime,`temp`) VALUES ('%s',(now()),'%s');",Idx,temp);
        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8","root","root");
            Statement st = con.createStatement();
            st.executeUpdate(sql);

        }catch(Exception e){
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        return map;
    }

    @ResponseBody
    @GetMapping("/searchCard")
    public Map<String, String> searchCard(@RequestParam String cardId,HttpServletResponse response){

        Map<String, String> map = new HashMap<>();
        int cnt = 0;

        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8","root","root");
            String qu = "select * from card where cardId=\'" + cardId + "\'";
            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(qu);

            while(rs.next()) {
                map.put("id", String.valueOf(rs.getInt("Idx")));
                map.put("grade", String.valueOf(rs.getInt("grade")));
                map.put("class", String.valueOf(rs.getInt("class")));
                map.put("classNumber", String.valueOf(rs.getInt("classNnumber")));
                map.put("name", rs.getString("name"));
                map.put("student", String.valueOf(rs.getBoolean("Idx")));
                map.put("cardId", rs.getString("cardId"));
                cnt++;
            }
            st.close();

        }catch(Exception e){
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        if(cnt == 0)
            response.setStatus(HttpServletResponse.SC_NOT_FOUND);
        else
            response.setStatus(HttpServletResponse.SC_ACCEPTED);
        return map;
    }

    @ResponseBody
    @GetMapping("/addCard")
    public Map<String, String> addCard(@RequestParam String grade,@RequestParam String class_,@RequestParam String number, @RequestParam String name,@RequestParam String student, @RequestParam String cardId){
        Map<String, String> map = new HashMap<>();
        String sql = String.format("INSERT INTO card (`grade`, `class`, `classNnumber`, `name`, `student`, `cardId`) VALUES ('%s', '%s', '%s', '%s', '%s', '%s');",grade,class_,number,name,student,cardId);
        try {
            Class.forName("com.mysql.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/ctc?useUnicode=true&characterEncoding=utf8","root","root");
            Statement st = con.createStatement();
            st.executeUpdate(sql);

        }catch(Exception e){
            System.err.println("Got an exception! ");
            System.err.println(e.getMessage());

        }

        return map;
    }

}
