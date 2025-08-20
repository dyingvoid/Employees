CREATE TABLE employee
(
    id            INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name          VARCHAR(60) NOT NULL,
    surname       VARCHAR(60) NOT NULL,
    phone         VARCHAR(12) NOT NULL,
    department_id BIGINT      NOT NULL,
    
    CONSTRAINT fk_employee_department FOREIGN KEY (department_id)
        REFERENCES department (id)
        ON DELETE CASCADE
);

CREATE INDEX idx_employee_department_id ON employee (department_id);