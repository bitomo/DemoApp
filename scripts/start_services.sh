#!/bin/bash
systemctl daemon-reload
systemctl start mvc.service
systemctl start razor.service