package com.bonatto.airline.controller;


import com.bonatto.airline.custumer.*;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;


@RestController
@RequestMapping("/custumer")
public class CustumerController {


    @Autowired
    private CustumerRepository custumerRepository;
    @PostMapping
    @Transactional
    public void register(@RequestBody @Valid CustumerRegisterData data)
    {
        custumerRepository.save(new Custumer(data));
    }

    @GetMapping("/list")
    public Page<CustumerDto> list (Pageable pageable)
    {
        return custumerRepository
                .findAll(pageable)
                .map( CustumerDto :: new);
    }

    @PutMapping("/update")
    @Transactional
    public void updateCustumer(@RequestBody @Valid CustumerUpdateDto data)
    {
        Custumer cus = custumerRepository.getReferenceById(data.id());
        cus.update(data);
    }

}
