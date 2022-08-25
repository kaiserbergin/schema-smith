import json
import logging
import sys

import jsonschema
import yaml
from jsonschema.validators import validate


def validate_schema(neo_schema):
    try:
        validation_schema = load_json('schema.json')
        validate(neo_schema, validation_schema)
        logging.info('schema validated successfully')
    except jsonschema.exceptions.ValidationError as err:
        logging.exception(err.message)
        sys.exit("SchemaSmith failed. Because you did.")


def load_json(file_name):
    with open(file_name, 'r') as json_schema_file:
        return json.load(json_schema_file)


def load_yaml(file_name):
    with open(file_name, 'r') as yaml_file:
        return yaml.safe_load(yaml_file)