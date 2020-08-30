﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BotoxSharedProtocol.Network.Fields
{
    public class ClassField
    {
        public string name { get; set; }
        public string type { get; set; }
        public string default_value { get; set; }
        public int? position { get; set; }

        public string self_serialize_method { get; set; }
        public string write_type_id_method { get; set; }
        public string write_method { get; set; }
        public string write_length_method { get; set; }
        public string write_false_if_null_method { get; set; }
        public Limits bounds { get; set; }
        public int? boolean_byte_wrapper_position { get; set; }
        public int? constant_length { get; set; }
        public bool is_vector { get; set; }
        public bool use_boolean_byte_wrapper { get; set; }
        public bool prefixed_by_type_id { get; set; }

        ~ClassField()
        {
            name = null;
            type = null;
            default_value = null;
            position = null;
            self_serialize_method = null;
            write_type_id_method = null;
            write_method = null;
            write_length_method = null;
            write_false_if_null_method = null;
            bounds = null;
            boolean_byte_wrapper_position = null;
            constant_length = null;

            use_boolean_byte_wrapper = false;
            is_vector = false;
            prefixed_by_type_id = false;
        }
    }
}
