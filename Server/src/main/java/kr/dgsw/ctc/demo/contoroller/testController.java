package kr.dgsw.ctc.demo.contoroller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class testController {

    @RequestMapping(value = "/test")
    public @ResponseBody
    String home(){
        return "index";
    }

}
