CREATE TABLE department
(
    id         BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name       VARCHAR(200) NOT NULL,
    phone      VARCHAR(15)  NOT NULL,
    company_id INT          NOT NULL,
    
    CONSTRAINT uq_department_name_company UNIQUE (name, company_id),
    CONSTRAINT fk_department_company FOREIGN KEY (company_id)
        REFERENCES company (id)
        ON DELETE CASCADE
);

CREATE INDEX idx_department_company_id ON department (company_id);
CREATE INDEX idx_department_name ON department (name);