import property_generator as generator

__INCOMING = "INCOMING"
__OUTGOING = "OUTGOING"


def __get_connection_array(connection_str):
    rel_pos = connection_str.find("-")
    n1 = connection_str[0:rel_pos]
    n2 = connection_str[rel_pos + 2:]
    direction = __OUTGOING if connection_str.find(">") > -1 else __INCOMING

    return [n1, direction, n2]


def __create_relationship_str(relationship, connection_count, connection_array):
    query = f'CREATE ({connection_array[0]})'
    query += '-' if connection_array[1] == __OUTGOING else '<-'
    query += f'[r{connection_count}:{relationship.name}]'
    query += '-' if connection_array[1] == __INCOMING else '->'
    query += f'({connection_array[2]}) '
    return query


def __set_properties_str(relationship, connection_count):
    query = ""
    if hasattr(relationship, 'properties'):
        for prop in relationship.properties:
            query += f'\nSET r{connection_count}.{prop.name} = {generator.generate_property(prop.type)}'

    return query


def generate_relationships_query(schema):
    query = ""
    connection_count = 0

    if hasattr(schema, 'relationships'):
        for relationship in schema.relationships:
            for connection in relationship.connections:
                connection_count += 1
                query += __create_relationship_str(relationship, connection_count, __get_connection_array(connection))
                query += __set_properties_str(relationship, connection_count)
                query += '\n\n'

    return query
