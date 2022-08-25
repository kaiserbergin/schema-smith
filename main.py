from schema_validator import *
from schema_converter import *
from node_query_generator import *
from relationship_query_generator import *
from constraint_query_generator import *
from index_generator import *


def replicate(neo_schema_file):
    neo_schema_yaml = load_yaml(neo_schema_file)
    validate_schema(neo_schema_yaml)

    schema = convert_schema_to_obj(neo_schema_yaml)
    print(generate_nodes_query(schema))
    print(generate_relationships_query(schema))
    print(generate_constraints_query(schema))
    print(generate_indexes_query(schema))


if __name__ == '__main__':
    replicate('schema.yaml')
