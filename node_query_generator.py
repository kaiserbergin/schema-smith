import property_generator as generator
from constants import *


def generate_nodes_query(schema):
    query = ""
    for node in schema.nodes:
        query += f'CREATE ({node.label}:{node.label}:{SCHEMA_SMITH_LABEL}) '

        if hasattr(node, 'properties'):
            for prop in node.properties:
                query += f'\nSET {node.label}.{prop.name} = {generator.generate_property(prop.type)} '

        query += '\n\n'

    return query
