CREATE TABLE passport
(
    number      VARCHAR(10) PRIMARY KEY,
    type        TEXT NOT NULL,
    employee_id INT  NOT NULL,

    CONSTRAINT fk_passport_employee FOREIGN KEY (employee_id)
        REFERENCES employee (id)
        ON DELETE CASCADE
);

CREATE INDEX idx_passport_employee_id ON passport (employee_id);